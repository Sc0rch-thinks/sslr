const { createClient } = supabase;

const supabaseUrl = "https://fchobpauqasfebohuuam.supabase.co";
const supabaseAnonKey =
	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZjaG9icGF1cWFzZmVib2h1dWFtIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzczNTYyNTUsImV4cCI6MjA1MjkzMjI1NX0.LkwCnzjtf8CPLrm6OONkjyjxZW8jE05V_spbOEeAXEM";

const Supabase = createClient(supabaseUrl, supabaseAnonKey);

const signInDialog = document.getElementById("signInDialog");
const signUpDialog = document.getElementById("signUpDialog");

var uid = "";

document.addEventListener("DOMContentLoaded", (event) => {
	Start();
});
document.getElementById("SignInSubmit").addEventListener("click", (e) => {
	e.preventDefault;
	var email = document.getElementById("emailSignIn").value;
	var password = document.getElementById("passwordSignIn").value;
	SignIn(email, password);
});
document.getElementById("SignUpSubmit").addEventListener("click", (e) => {
	e.preventDefault;
	var email = document.getElementById("emailSignUp").value;
	var password = document.getElementById("passwordSignUp").value;
	var username = document.getElementById("usernameSignUp").value;
	var fileInput = document.getElementById("profileSignUp");
	const file = fileInput.files[0];

	SignUp(email, password, username, file);
});
document.getElementById("signUpToSignIn").addEventListener("click", () => {
	ShowSignIn();
});
document.getElementById("signinToSignup").addEventListener("click", () => {
	ShowSignUp();
});

function ShowSignIn() {
	signUpDialog.close();
	signInDialog.showModal();
	console.log("sign in showed");
}
function ShowSignUp() {
	signInDialog.close();
	signUpDialog.showModal();
	console.log("sign up showed");
}
function closeDialog() {
	signInDialog.close();
	signUpDialog.close();
}
async function Start() {
	const { data } = await Supabase.auth.getSession();
	console.log(data);
	if (data.session == null || data.session == "") {
		uid = "";
		ShowSignUp();
	} else {
		uid = data.session.user.id;
	}
}

async function SignIn(email, password) {
	console.log(email, password);

	const { data: auth, error: authError } = await Supabase.auth.signInWithPassword({
		email: email,
		password: password,
	});

	if (authError) {
		console.error("Error signing in:", authError.message);
		return;
	}

	console.log("User signed in:", auth);
	uid = auth.user.id;
	const { data, error } = await Supabase.from("users").select();
	console.log(data);
	if (error) {
		console.error("Error getting data:", error.message);
		return;
	}
	closeDialog();
}

async function SignUp(email, password, username, file) {
	console.log(email, password);
	const { data, error: authError } = await Supabase.auth.signUp({
		email: email,
		password: password,
	});

	uid = data.user.id;
	if (file.size > 1e7) {
		console.error("file too fat");
	}
	const fileName = `${Date.now()}-${file.name}`;

	const { storeData, storeError } = await Supabase.storage
		.from("Avatar") // Replace 'images' with your bucket name
		.upload(fileName, file);
	if (storeError) {
		console.error("Upload failed:", storeError.message);
		return;
	}

	const publicUrlData = Supabase.storage.from("Avatar").getPublicUrl(fileName);
	const publicUrl = publicUrlData.data.publicUrl;
	console.log("Public URL:", publicUrl);
	const { error } = await Supabase.from("users").insert({
		uid: uid,
		displayName: username,
		profilePictureUrl: publicUrl,
	});
}
