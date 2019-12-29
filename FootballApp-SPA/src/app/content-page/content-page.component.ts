import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-content-page',
    templateUrl: './content-page.component.html',
    styleUrls: ['./content-page.component.css']
})
export class ContentPageComponent {
    @Input() pageToDisplay = 'dashboard';
}