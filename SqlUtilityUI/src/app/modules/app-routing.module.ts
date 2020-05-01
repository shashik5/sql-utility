import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DesignerComponent } from './designer/designer.component';
import { HomeComponent } from './home/home.component';
import { TutorialComponent } from './tutorial/tutorial.component';
import { ContactUsComponent } from './contactUs/contact-us.component';


const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'designer', component: DesignerComponent },
    { path: 'tutorial', component: TutorialComponent },
    { path: 'contact', component: ContactUsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: false
    })],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
