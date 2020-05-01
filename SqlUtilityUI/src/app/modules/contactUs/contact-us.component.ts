import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

const EMAIL_REGEX = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

@Component({
    selector: 'contact-us',
    templateUrl: './contact-us.component.html',
    styleUrls: ['./contact-us.component.scss']
})

export class ContactUsComponent {
    private contactFormData = {
        Name: '',
        Email: '',
        Message: '',
        FileData: null,
        FileName: ''
    };

    private submit = function () {
        debugger;
        //this.validateForm();
    };

    private validateForm = function () {
        var isValid = true;

        if (!this.contactFormData.Name || this.contactFormData.Email || this.contactFormData.Message) {
            isValid = false;
        };

        if (this.contactFormData.Email) {
            isValid = EMAIL_REGEX.test(this.contactFormData.Email);
        };

        return isValid;
    };

    private emailFormControl = new FormControl('', [
        Validators.pattern(EMAIL_REGEX)
    ]);

    private openFileSelector = function () {
        var fileControl = document.createElement('input');
        fileControl.setAttribute('type', 'file');

        fileControl.onchange = (e: any) => {
            try {
                var file = e.target.files[0];
                if (file) {
                    var reader = new FileReader();
                    reader.onload = (res: any) => {
                        this.contactFormData.FileData = res.target.result;
                        this.contactFormData.FileName = file.name;
                    };

                    reader.readAsArrayBuffer(file);
                };
            } catch (e) {

            }
        };

        fileControl.click();
    };
}
