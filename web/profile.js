




const signInDialog = document.getElementById("signInDialog");
const signUpDialog = document.getElementById("signUpDialog");
document.addEventListener("DOMContentLoaded", (event) => {});

function ShowSignIn() {
  // signUpDialog.close();
	signInDialog.showModal();
	console.log("sign in showed");
}
function ShowSignUp() {
  signInDialog.close()
  signUpDialog.showModal()
  console.log("sign up showed")
  
}

