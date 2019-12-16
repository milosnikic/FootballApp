import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatSliderModule } from '@angular/material/slider';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
   declarations: [
      AppComponent,
      RegisterComponent,
   ],
   imports: [
      HttpClientModule,
      BrowserModule,
      BrowserAnimationsModule,
      MatSliderModule,
      FormsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
