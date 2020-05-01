import { Component } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})

export class HomeComponent {
    private whyUs = ['No need to install database server on your machine.', 'Need not worry about hardware support.', 'Just need browser and internet to play around.'];
    private features = ['Easy to use interface.', 'Supports CREATE, ADD, UPDATE, DELETE, DROP and SELECT queries.', 'Design your SQL queries by just drag and drop.'];
}
