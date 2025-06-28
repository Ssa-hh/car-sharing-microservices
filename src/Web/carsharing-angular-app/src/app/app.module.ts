import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient  } from '@angular/common/http';
import { NgbModule, NgbNavModule, NgbCollapseModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxBootstrapIconsModule } from 'ngx-bootstrap-icons';
import { search, personCircle, boxArrowRight, carFrontFill, calendar3, plusCircle, chevronRight } from 'ngx-bootstrap-icons';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { RegisterComponent } from './features/auth/components/register/register.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';
import { ToastsContainerComponent } from './shared/toast/toasts-container/toasts-container.component';
import { SearchRidesComponent } from './features/rides/components/search-rides/search-rides.component';
import { SearchRidesFormComponent } from './features/rides/components/search-rides-form/search-rides-form.component';
import { RidesItemComponent } from './features/rides/components/rides-item/rides-item.component';
import { AddRideComponent } from './features/rides/components/add-ride/add-ride.component';

const icons = {
  search,
  personCircle,
  boxArrowRight,
  carFrontFill,
  calendar3,
  plusCircle,
  chevronRight
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FooterComponent,
    RegisterComponent,
    LoginComponent,
    LoadingSpinnerComponent,
    ToastsContainerComponent,
    SearchRidesComponent,
    SearchRidesFormComponent,
    RidesItemComponent,
    AddRideComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    NgbNavModule,
    NgbCollapseModule,
    NgxBootstrapIconsModule.pick(icons),
    ReactiveFormsModule,
    NgbTooltipModule,
    NgbDatepickerModule
  ],
  providers: [
    provideHttpClient()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
