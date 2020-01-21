import { Directive, HostBinding, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: '[appSetActive]'
})
export class SetActiveDirective {

  constructor(private _elementRef: ElementRef) { }

  @HostBinding('class') active = 'user';

  @HostListener('document:click', ['$event.target']) onClick(targetElement) {
    const clickedInside = this._elementRef.nativeElement.contains(targetElement);
    if(!clickedInside){
      this.active = 'user';
    }else{
      this.active = 'user active';
    }
  }
}
