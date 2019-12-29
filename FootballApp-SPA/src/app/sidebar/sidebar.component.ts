import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {

    selectedItem = 'dashboard';
    @Output() itemToDisplay = new EventEmitter<string>();
    constructor() {}

    onSelect(item: string){
        this.selectedItem = item === 'home' ? 'dashboard' : item;
        this.itemToDisplay.emit(this.selectedItem);
    }
}
