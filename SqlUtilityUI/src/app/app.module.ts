import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { ContextMenuModule } from 'ngx-contextmenu';
import { NgDragDropModule } from 'ng-drag-drop';
import { PerfectScrollbarModule, PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatTooltipModule, MatSelectModule, MatExpansionModule } from '@angular/material';

import { MainMenuComponent } from './modules/mainMenu/main-menu.component';
import { CustomFooterComponent } from './modules/customFooter/custom-footer.component';
import { HomeComponent } from './modules/home/home.component';
import { DesignerComponent } from './modules/designer/designer.component';
import { TutorialComponent } from './modules/tutorial/tutorial.component';
import { ContactUsComponent } from './modules/contactUs/contact-us.component';

import { AppRoutingModule } from './modules/app-routing.module';
import { WebApiRequestService } from './webApiRequestService';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
    suppressScrollX: true
};

@NgModule({
    declarations: [
        AppComponent,
        MainMenuComponent,
        CustomFooterComponent,
        HomeComponent,
        DesignerComponent,
        TutorialComponent,
        ContactUsComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatTooltipModule,
        MatSelectModule,
        MatExpansionModule,
        PerfectScrollbarModule,
        HttpClientModule,
        ContextMenuModule,
        NgDragDropModule.forRoot()
    ],
    providers: [
        {
            provide: PERFECT_SCROLLBAR_CONFIG,
            useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
        },
        WebApiRequestService,
        CookieService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
    constructor(private cookieService: CookieService) {
        if (window.location.hostname == 'localhost') {
            var cookie = cookieService.get('SU_Auth');
            if (!cookie) {
                var defaultCookie = {
                    sessionId: 'localhostsession'
                };
                cookieService.set('SU_Auth', JSON.stringify(defaultCookie));
            };
        };
    }
}
