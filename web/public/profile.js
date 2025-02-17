const { createClient } = supabase;

const supabaseUrl = "https://fchobpauqasfebohuuam.supabase.co";
const supabaseAnonKey =
	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZjaG9icGF1cWFzZmVib2h1dWFtIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzczNTYyNTUsImV4cCI6MjA1MjkzMjI1NX0.LkwCnzjtf8CPLrm6OONkjyjxZW8jE05V_spbOEeAXEM";

const Supabase = createClient(supabaseUrl, supabaseAnonKey);

const signInDialog = document.getElementById("signInDialog");
const signUpDialog = document.getElementById("signUpDialog");

var uid = "";

function userInfo(url, name, Score, daysPlayed, helpedCorrectly, helpedWrongly) {
	this.url = url;
	this.name = name;
	this.Score = Score;
	this.daysPlayed = daysPlayed;
	this.helpedCorrectly = helpedCorrectly;
	this.helpedWrongly = helpedWrongly;
}

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
	const { data: authData } = await Supabase.auth.getSession();
	if (authData.session == null || authData.session == "") {
		uid = "";
		ShowSignUp();
		return
	}
	uid=authData.session.user.id
	console.log(authData.session.user.id);
	const { data, error } = await Supabase.from("users").select().eq("uid", uid);
	console.log(data);
	if (error) {
		console.error("Error getting data:", error.message);
		return;
	}
	var tempData=data[0];
	var temp = new userInfo(
		tempData.profilePictureUrl,
		tempData.displayName,
		tempData.score,
		tempData.daysPlayed,
		tempData.customersHelpedWrongly,
		tempData.customersHelpedCorrectly
	);
	closeDialog();
	showData(temp)
	
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
	const { data, error } = await Supabase.from("users").select().eq("uid", uid);
	console.log(data);
	if (error) {
		console.error("Error getting data:", error.message);
		return;
	}
	var tempData=data[0]
	var temp = new userInfo(
		tempData.profilePictureUrl,
		tempData.displayName,
		tempData.score,
		tempData.daysPlayed,
		tempData.customersHelpedWrongly,
		tempData.customersHelpedCorrectly
	);
	closeDialog();
	showData(temp)
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
	var temp=new userInfo(
		publicUrl,
		username,
		0,
		0,
		0,
		0,
	)
	showData(temp)
}
function showData(userInfo) {
	const profile = document.getElementById("profile");
	console.log(profile.outerHTML);
	var Accuracy=(userInfo.helpedCorrectly / (userInfo.helpedCorrectly + userInfo.helpedWrongly)) *
	100
	if(Accuracy==NaN)
	{
		Accuracy=0
	}

	profile.outerHTML = `<section class="h-fit w-full py-10 px-[15vw] "  id="profile">
			<b>Profile</b>
			<img src="${userInfo.url}" class="h-40 w-40 rounded-full" alt="Profile picture" />
			<h1><b>Name:</b>${userInfo.name}</h1>
			<h1><b>Score:</b>${userInfo.Score}</h1>
			<h1><b>Days Played:</b>${userInfo.daysPlayed}</h1>
			<h1><b>Customers Helped Correct:</b>${userInfo.helpedCorrectly}</h1>
			<h1><b>Customers Helped Wrongly:</b>${userInfo.helpedWrongly}</h1>
			<h1><b>Accuracy:</b>${
				Accuracy
			}%</h1>
		</section>`;
}
