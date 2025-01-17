# sota staircase ReStepper
# forge -> github one-way repo sync script
# licence: 0BSD
from multiprocessing.pool import ThreadPool
from os import getenv
from pathlib import Path
from pprint import pformat
from shutil import copy2, copytree
from subprocess import CompletedProcess
from subprocess import run as _run
from sys import argv, executable
from tempfile import TemporaryDirectory
from textwrap import indent
from time import time
from traceback import format_tb
from typing import Callable, Final, TypeVar

try:
    from sidestepper import (
        SOTA_SIDESTEP_MAX_WORKERS,
        LargeFileFilterResult,
        find_large_files,
        generate_command_failure_message,
        generate_time_elapsed_string,
        iter_files,
        run,
        write_sotaignore,
    )
except EnvironmentError:
    # specific error raised when third-party modules not found, but were automatically
    # installed, so we need to restart the script
    exit(_run([executable, Path(__file__).absolute(), *argv[1:]]).returncode)

# we can only guarantee third-party modules are installed after sidestepper
from tqdm import tqdm

# constants
INDENT: Final[str] = "   "
REPO_DIR: Final[Path] = Path(__file__).parent
REPO_SOTAIGNORE: Final[Path] = REPO_DIR.joinpath(".sotaignore")
REPO_URL_GITHUB: Final[str] = "github.com/Sc0rch-thinks/sslr"
REPO_URL_FORGE: Final[str] = "forge.joshwel.co/Ryan/SSLR"
COMMIT_MESSAGE: Final[str] = "chore(restep): sync with forge"
COMMIT_AUTHOR: Final[str] = "sota staircase ReStepper <ssrestepper@joshwel.co>"
NEUTERED_GITATTRIBUTES: Final[str] = (
    """# auto detect text files and perform lf normalization\n* text=auto\n"""
)
GH_ACT: Final[bool] = getenv("GITHUB_ACTIONS", "").lower() == "true"
GH_TOKEN: Final[str] = getenv("SS_RESTEPPER_TOKEN", "")
GH_USERNAME: Final[str] = "Sc0rch-thinks"
if GH_ACT and GH_TOKEN == "":
    print(
        "critical error: no personal access token found in SS_RESTEP_TOKEN, "
        "may not have permission to push to github"
    )
    exit(1)

# dictionary to share state across steps
r: dict[str, str] = {}

R = TypeVar("R")


class CopyHighway:
    """
    multithreaded file copying class that gives a copy2-like function
    for use with shutil.copytree(); also displays a progress bar
    """

    pool: ThreadPool
    pbar: tqdm
    lff_result: LargeFileFilterResult | None
    respect_ignore: bool = True

    def __init__(
        self, message: str, total: int, lff_result: LargeFileFilterResult | None
    ):
        """
        multithreaded file copying class that gives a copy2-like function
        for use with shutil.copytree()

        args:
            message: str
                message to display in the progress bar
            total: int
                total number of files to copy
            lff_result: LargeFileFilterResult
                result of the large file filter
        """
        self.pool = ThreadPool(
            processes=SOTA_SIDESTEP_MAX_WORKERS,
        )
        self.pbar = tqdm(
            total=total,
            desc=message,
            unit=" files",
            leave=False,
        )
        self.lff_result = lff_result
        self.respect_ignore = False if "--dupethelongway" in argv else True

    def callback(self, a: R):
        self.pbar.update()
        return a

    def copy2(self, source: Path | str, dest: Path | str) -> None:
        """shutil.copy2()-like function for use with shutil.copytree()"""

        if self.respect_ignore and (self.lff_result is not None):
            # ignore check 1: dir
            for ign_dir in self.lff_result.ignore_directories:
                if str(ign_dir) in str(source):
                    self.pbar.update()
                    return None

            # ignore check 2: file
            # ... we don't need to use the trytrytry method
            # ... because we already did that as part of the large file filter,
            # ... and as such we checked for it with the first check above
            if self.lff_result.matcher.match(source):
                self.pbar.update()
                return None

        self.pool.apply_async(copy2, args=(source, dest), callback=self.callback)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.pool.close()
        self.pool.join()
        self.pbar.close()


def _default_post_func(cp: R) -> R:
    """
    default post-call function for steps; does nothing

    for steps that return a CompletedProcess, this function will run the
    `_command_post_func` function

    args:
        cp: R
            return object from a step function

    returns: R
        the return object from the step function
    """
    if isinstance(cp, CompletedProcess):
        _command_post_func(cp)
    return cp


def _command_post_func(
    cp: CompletedProcess,
    fail_on_error: bool = True,
    quit_early: bool = False,
    quit_message: str = "the command gave unexpected output",
) -> CompletedProcess:
    """
    default post-call function for command steps; checks if the command was
    successful and prints the output if it wasn't

    if the command was successful, the stdout and stderr are stored in the
    shared state dictionary r under 'stdout' and 'stderr' respectively

    args:
        cp: CompletedProcess
            return object from subprocess.run()
        fail_on_error: bool
            whether to fail on error
        quit_early: bool
            whether to quit early
        quit_message: str
            the message to print if quitting early

    returns: CompletedProcess
        the return object from subprocess.run()
    """

    if quit_early:
        print(f"\n\nfailure: {quit_message}\n")

    else:
        r["stdout"] = cp.stdout.decode() if isinstance(cp.stdout, bytes) else "\0"
        r["stderr"] = cp.stderr.decode() if isinstance(cp.stderr, bytes) else "\0"
        r["blank/stdout"] = "yes" if (r["stdout"].strip() == "") else ""
        r["blank/stderr"] = "yes" if (r["stderr"].strip() == "") else ""
        r["blank"] = "yes" if (r["blank/stdout"] and r["blank/stderr"]) else ""
        r["errored"] = "" if (cp.returncode == 0) else str(cp.returncode)

        # return if the command was successful
        # or if we're not failing on error
        if (cp.returncode == 0) or (not fail_on_error):
            return cp
        else:
            print(generate_command_failure_message(cp))

    exit(
        cp.returncode if (isinstance(cp.returncode, int) and cp.returncode != 0) else 1
    )


def post_filter_repo_check(cp: CompletedProcess) -> CompletedProcess:
    """
    post-call function for checking if git-filter-repo is installed
    and optionally installing it if it isn't
    """

    if cp.returncode == 0:
        return cp

    if input("git filter-repo is not installed, install it? y/n: ").lower() != "y":
        print(
            "install it using 'pip install git-filter-repo' "
            "or 'pipx install git-filter-repo'",
        )
        return cp

    # check if pipx is installed
    use_pipx = False

    check_pipx_cp = run(["pipx", "--version"])
    if check_pipx_cp.returncode == 0:
        use_pipx = True

    # install git-filter-repo
    pip_invocation: list[str] = ["pipx"] if use_pipx else [executable, "-m", "pip"]
    print(
        f"running '{' '.join([*pip_invocation, 'install', 'git-filter-repo'])}'... ",
        end="",
        flush=True,
    )
    install_rc = run([*pip_invocation, "install", "git-filter-repo"])
    if install_rc.returncode != 0:
        print("error")
        _command_post_func(install_rc)
        exit(install_rc.returncode)
    else:
        print("done\n")

    # check if it is reachable
    if run(["git", "filter-repo", "--version"]).returncode != 0:
        # revert
        run([*pip_invocation, "uninstall", "git-filter-repo"])
        print(
            "failure: could not install git-filter-repo automatically. "
            "do it yourself o(*≧▽≦)ツ┏━┓"
        )
        exit(-1)

    return cp


def rewrite_gitattributes(target_dir: Path) -> None:
    """
    rewrite the .gitattributes file in a directory to disable git-lfs

    args:
        target_dir: Path
            the directory to search
    """

    # recursively search for .gitattributes files
    for repo_file in target_dir.rglob(".gitattributes"):
        repo_file.write_text(NEUTERED_GITATTRIBUTES, encoding="utf-8")


def step(
    func: Callable[[], R],
    desc: str = "",
    post_func: Callable[[R], R] = _default_post_func,
    post_print: bool = True,
) -> R:
    """
    helper function for running steps

    args:
        desc: str
            description of the step
        func: Callable[[], R]
            function to run
        post_func: Callable[[R], R]
            post-function to run after func
        post_print: bool
            whether to print done after the step

    returns:
        R
            return object from func
    """

    # run the function
    if desc != "":
        print(f"{desc}..", end="", flush=True)

    start_time = time()

    try:
        cp = func()

    except Exception as exc:
        print(
            f"\n\nfailure running step: {exc} ({exc.__class__.__name__})",
            "\n".join(format_tb(exc.__traceback__)) + "\n",
            sep="\n",
        )
        exit(1)

    if desc != "":
        print(".", end="", flush=True)

    # run the post-function
    try:
        rp = post_func(cp)

    except Exception as exc:
        print(
            f"\n\nfailure running post-step: {exc} ({exc.__class__.__name__})",
            "\n".join(format_tb(exc.__traceback__)) + "\n",
            sep="\n",
        )
        exit(1)

    end_time = time()

    # yay
    if desc != "" and post_print:
        print(
            f" done in {generate_time_elapsed_string(end_time - start_time)}",
            flush=True,
        )

    return rp


def post_remote_v(cp: CompletedProcess) -> CompletedProcess:
    """
    post-call function for 'git remote -v' command, parses the output and
    checks for the forge and github remotes, storing them in the shared state
    under 'remote/forge', 'remote/forge/url', 'remote/github', and
    'remote/github/url' respectively
    """

    if not isinstance(cp.stdout, bytes):
        return _command_post_func(cp)

    for line in cp.stdout.decode().split("\n"):
        # github  https://github.com/markjoshwel/sota (fetch)
        # github  https://github.com/markjoshwel/sota (push)
        # origin  https://forge.joshwel.co/mark/sota.git (fetch)
        # origin  https://forge.joshwel.co/mark/sota.git (push)

        split_line = line.split(maxsplit=1)
        if len(line) < 2:
            continue

        # remote='origin'  url='https://forge.joshwel.co/mark/sota.git (fetch)'
        remote, url = split_line

        # clean up the url
        if (REPO_URL_FORGE in url) or (REPO_URL_GITHUB in url):
            # url='https://forge.joshwel.co/mark/sota.git'
            url = url.split("(", maxsplit=1)[0].strip()

        if REPO_URL_FORGE in url:
            r["remote/forge"] = remote
            r["remote/forge/url"] = url

        elif REPO_URL_GITHUB in url:
            r["remote/github"] = remote
            r["remote/github/url"] = url

    return _command_post_func(cp)


def err(message: str, exc: Exception | None = None) -> None:
    """
    helper function for printing error messages, prints the message and the
    shared state dictionary r

    args:
        message: str
            the error message to print
        exc: Exception | None
            the exception that caused the error, if any
    """

    print(
        "\n" + message,
        (
            ""
            if (exc is None)
            else indent(
                text=(
                    f"{exc} ({exc.__class__.__name__})\n"
                    f"{'\n'.join(format_tb(exc.__traceback__))}\n"
                ),
                prefix=INDENT,
            )
        )
        + (indent(text=pformat(r), prefix=INDENT) + "\n"),
        sep="\n",
    )
    exit(1)


def main() -> None:
    """
    command line entry point
    """

    cumulative_start_time = time()
    with TemporaryDirectory(delete="--keep" not in argv) as dir_temp:
        print(
            "\nsota staircase ReStepper\n"
            "\n"
            "directories\n"
            f"   real repo : {REPO_DIR}\n"
            f"   temp repo : {dir_temp}\n",
            f"   is gh act : {GH_ACT}\n" if GH_ACT else "",
            sep="",
        )

        # helper partial function for command
        def cmd(
            command: str,
            wd: Path | str = dir_temp,
            capture_output: bool = True,
            give_input: str | None = None,
        ) -> Callable[[], CompletedProcess]:
            return lambda: run(
                command,
                cwd=wd,
                capture_output=capture_output,
                give_input=give_input,
            )

        step(
            func=cmd("git filter-repo --version"),
            post_func=post_filter_repo_check,
        )

        step(cmd("git status --porcelain", wd=REPO_DIR))
        if (not r["blank"]) and ("--iknowwhatimdoing" not in argv):
            err(
                "critical error: repository is not clean, please commit changes first",
            )

        start_time = time()
        print("1 pre | finding large files", end="", flush=True)
        files, sim = iter_files(REPO_DIR)

        flf_filter_result: LargeFileFilterResult | None = None
        if "--skipsotaignoregen" not in argv:
            flf_filter_result = find_large_files(files, sim)
            large_files = flf_filter_result.files
            end_time = time()
            print(
                "1 pre | finding large files... "
                f"done in {generate_time_elapsed_string(end_time - start_time)} (found {len(large_files)})"
            )

            if large_files:
                start_time = time()
                was_written = step(
                    desc="2 pre | writing .sotaignore",
                    func=lambda: write_sotaignore(large_files),
                    post_func=lambda cp: cp,
                    post_print=False,
                )
                end_time = time()
                if was_written:
                    print(
                        f" done in {generate_time_elapsed_string(end_time - start_time)}"
                    )
                else:
                    print(" not needed")
        else:
            end_time = time()
            print(
                "1 pre | finding large files... "
                f"skipped in {generate_time_elapsed_string(end_time - start_time)}"
            )

        print("3 pre | duplicating repo... pre-scanning", end="", flush=True)

        start_time = time()
        with CopyHighway(
            message="3 pre | duplicating repo",
            total=len(list(REPO_DIR.rglob("*"))),
            lff_result=flf_filter_result,
        ) as copier:
            copytree(
                src=REPO_DIR,
                dst=dir_temp,
                copy_function=copier.copy2,
                dirs_exist_ok=True,
            )
        end_time = time()
        print(
            f"3 pre | duplicating repo... done in {generate_time_elapsed_string(end_time - start_time)}",
            flush=True,
        )

        step(cmd('python -c "import pathlib; print(pathlib.Path.cwd().absolute())"'))
        if str(Path(dir_temp).absolute()) != r["stdout"].strip():
            err(
                "critical error (whuh? internal?): "
                f"not inside the temp dir '{str(Path(dir_temp).absolute())}'"
            )

        # check for forge and github remotes
        step(
            func=cmd("git remote -v"),
            post_func=post_remote_v,
        )
        if "remote/forge" not in r:
            err("critical error (whuh?): no forge remote found")

        # get the current branch
        step(cmd("git branch --show-current"))
        branch = r["stdout"].strip()
        if r.get("errored", "yes") or branch == "":
            err("critical error (whuh?): couldn't get current branch")

        step(cmd(f"git fetch {r['remote/forge']}"))
        step(cmd(f"git rev-list HEAD...{r['remote/forge']}/{branch} --count"))
        if (r.get("stdout", "").strip() != "0") and ("--dirty" not in argv):
            err(
                "critical error (whuh?): "
                "not up to date with forge... sync your changes first?"
            )

        step(desc="4 lfs | fetch lfs objects", func=cmd("git lfs fetch"))

        step(
            desc="5 lfs | migrating lfs objects",
            func=cmd(
                'git lfs migrate export --everything --include="*" --remote=origin',
                give_input="y\n",
            ),
        )

        step(
            desc="6 lfs | uninstall lfs in repo",
            func=cmd("git lfs uninstall"),
        )

        step(
            func=cmd("git lfs ls-files"),
        )
        if not r["blank"]:
            err(
                "critical error (whuh? internal?): "
                "lfs objects still exist post-migrate and uninstall"
            )

        if REPO_SOTAIGNORE.exists():
            try:
                sotaignore = REPO_SOTAIGNORE.read_text(encoding="utf-8").strip()
            except Exception as exc:
                err("critical error: couldn't read .sotaignore file", exc=exc)

            sotaignored_files: list[str] = [
                line
                for line in sotaignore.splitlines()
                if not line.startswith("#") and line.strip() != ""
            ]

            step(
                desc=f"7 lfs | filter repo and {len(sotaignored_files)} file(s)",
                func=cmd(
                    "git filter-repo --force --strip-blobs-bigger-than 100M --invert-paths "
                    + " ".join(f'--path ""{lf}' "" for lf in sotaignored_files)
                ),
            )

            # also copy to the temp repo; step 5 (lfs migrate) wipes uncommitted changes
            copy2(REPO_SOTAIGNORE, Path(dir_temp).joinpath(".sotaignore"))

        def add_and_commit() -> CompletedProcess:
            if GH_ACT:
                cp = cmd("git config user.name 'github-actions[bot]'")()
                if cp.returncode != 0:
                    return cp

                cp = cmd(
                    "git config user.email 'github-actions[bot]@users.noreply.github.com'"
                )()
                if cp.returncode != 0:
                    return cp

            cp = cmd("git add -A")()
            if cp.returncode != 0:
                return cp

            return cmd(
                "git commit --allow-empty "
                f'-am "{COMMIT_MESSAGE}" --author="{COMMIT_AUTHOR}"',
            )()

        def neuter_and_commit():
            rewrite_gitattributes(Path(dir_temp))
            add_and_commit()

        step(
            desc="8 fin | neuter .gitattributes and commit",
            func=neuter_and_commit,
        )

        if r.get("remote/github") is None:
            step(
                func=cmd(f"git remote add github https://{REPO_URL_GITHUB}.git"),
            )
            if r.get("errored", "yes"):
                err("critical error (whuh?): couldn't add github remote")
            r["remote/github"] = "github"

        step(
            desc=f"9 fin | fetch {r['remote/github']}",
            func=cmd(f"git fetch {r['remote/github']}"),
        )

        push_invocation = (
            f"git push {r['remote/github']} {branch} --force"
            if not GH_ACT
            else f"git push https://{GH_USERNAME}:{GH_TOKEN}@{REPO_URL_GITHUB}.git {branch} --force"
        )

        step(
            desc=f"X fin | pushing to {r['remote/github']}/{branch}",
            func=cmd(push_invocation if ("--test" not in argv) else "git --version"),
        )

    cumulative_end_time = time()
    print(
        f"\n--- done! took {generate_time_elapsed_string(cumulative_end_time - cumulative_start_time)}~ "
        "☆*: .｡. o(≧▽≦)o .｡.:*☆ ---",
        flush=True,
    )


if __name__ == "__main__":
    main()
