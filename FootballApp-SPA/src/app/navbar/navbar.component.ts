import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

    @ViewChild('searchBox', {'static': true}) searchBox: ElementRef;

    constructor(private router: Router){}
    onClear(){
        this.searchBox.nativeElement.value = '';
    }

    logout(){
        localStorage.clear();
        this.router.navigate(['']);
    }

}