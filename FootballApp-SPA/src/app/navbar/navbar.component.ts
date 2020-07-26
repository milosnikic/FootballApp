import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models/user';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{
    user: User;
    @ViewChild('searchBox', {'static': true}) searchBox: ElementRef;

    constructor(private router: Router){}

    ngOnInit(): void {
        this.user = JSON.parse(localStorage.getItem('user'));
    }
    onClear(){
        this.searchBox.nativeElement.value = '';
    }

    logout(){
        localStorage.clear();
        this.router.navigate(['']);
    }

}