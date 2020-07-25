import { Directive, HostBinding, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: '[appSetActive]'
})
export class SetActiveDirective {

  constructor(private elementRef: ElementRef) { }

  @HostBinding('class') active = 'user';

  @HostListener('document:click', ['$event.target']) onClick(targetElement) {
    const clickedInside = this.elementRef.nativeElement.contains(targetElement);
    console.log(this.elementRef);
    console.log(targetElement);
    // Here we have to leave blue bar on left side
    // also when user clicks on input field for message
    if (!clickedInside) {
      this.active = 'user';
    } else if(clickedInside){
      this.active = 'user active';
    }
  }
}