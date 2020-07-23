import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
/**
 * Notify service is used to encapsulate toast service
 */
export class NotifyService {
  constructor(private toastrService: ToastrService) {}

  /**
   * 
   * @param title Title of message
   * @param message Content of message
   * @description Method used for showing toast info
   */
  showInfo(
    message: string = 'This is information message!',
    title: string = 'Information'
  ) {
    this.toastrService.info(message, title);
  }
  /**
   * 
   * @param title Title of message
   * @param message Content of message
   * @description Method used for showing toast warning
   */
  showWarning(
    message: string = 'Be carefull doing this!',
    title: string = 'Warning'
  ) {
    this.toastrService.warning(message, title);
  }
  /**
   * 
   * @param title Title of message
   * @param message Content of message
   * @description Method used for showing toast error
   */
  showError(
    message: string = 'Something went wrong!',
    title: string = 'Error'
  ) {
    this.toastrService.error(message, title);
  }
  /**
   * 
   * @param title Title of message
   * @param message Content of message
   * @description Method used for showing toast success
   */
  showSuccess(
    message: string = 'Successfully done!',
    title: string = 'Success'
  ) {
    this.toastrService.success(message, title);
  }
}
