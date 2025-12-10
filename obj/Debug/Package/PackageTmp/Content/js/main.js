AOS.init({
	duration: 800,
	easing: 'slide',
	once: true
});



jQuery(document).ready(function ($) {

	"use strict";
	let email = ""
	let subject = ""
	let message = ""

	const sendBtnNode = document.getElementById("send")
	const toastSuccess = $('#toast_success')
	const toastDanger = $('#toast_danger')
	toastSuccess.hide()
	toastDanger.hide()

	function setFieldValues(id, value) {
		switch (id) {
			case "email_address":
				email = value
				break;
			case "email_subject":
				subject = value
				break;
			case "email_message":
				message = value
				break;

			default: console.log("error")
				break;
		}
	}



	function sendEmail() {

		if (email.length > 0 && subject.length > 0 && message.length > 0) {
			Email.send({
				SecureToken: "14ffee4b-e850-4b49-a155-52e9b1ad5997",
				To: 'soporteauditoria@pacifico.com.pe',
				From: email,
				Subject: subject,
				Body: message
			}).then(
				() => {
					toastSuccess.show()
					setTimeout(() => { toastSuccess.hide() }, 2500);
				}
			)
		}
		else {

			toastDanger.show()
			setTimeout(() => { toastDanger.hide() }, 1000);
		}

	}

	const form = document.getElementById("contact");
	sendBtnNode.addEventListener("click", () => { sendEmail() })
	form.addEventListener("input", (event) => {
		setFieldValues(event.target.id, event.target.value)
	})

});