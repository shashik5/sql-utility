import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'main-menu',
    templateUrl: './main-menu.component.html',
    styleUrls: ['./main-menu.component.scss']
})

export class MainMenuComponent {
    private menuItems = [];
    private appName = 'SQL Utility';

    constructor(private router: Router) {
        this.addMenuItem('Home', 'home', 'Home');
        this.addMenuItem('Designer', 'designer', 'Designer');
        this.addMenuItem('Tutorial', 'tutorial', 'Tutorial');
        this.addMenuItem('Contact Us', 'contact', 'Contact');
    }

    private addMenuItem(name: string, redirectUrl: string, iconClass: string, isIconRequired: boolean = true): void {
        var item = new MenuItem();

        if (isIconRequired && !iconClass) {
            isIconRequired = false;
        };

        item.name = name || '';
        item.redirectUrl = redirectUrl || '';
        item.iconClass = iconClass;
        item.isIconRequired = isIconRequired;

        this.menuItems.push(item);
    }
}

class MenuItem {
    name: string;
    redirectUrl: string;
    iconClass: string;
    isIconRequired: boolean;
}
