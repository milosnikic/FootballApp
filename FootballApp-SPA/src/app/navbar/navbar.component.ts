import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

    @ViewChild('searchBox', {'static': true}) searchBox: ElementRef;

    onClear(){
        this.searchBox.nativeElement.value = '';
    }

    logout(){
        localStorage.clear();
    }

}