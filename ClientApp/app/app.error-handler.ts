import { ErrorHandler, Inject } from '@angular/core';
import { ToastyService } from "ng2-toasty";

export class AppErrorHandler implements ErrorHandler {

    /**
     *
     */
    constructor(@Inject(ToastyService)private toastyService: ToastyService) {
    }

    handleError(error: any): void {
        this.toastyService.error({
            title: 'Error',
            msg: 'An unexpected error happened.',
            theme: 'bootstrap',
            showClose: true,
            timeout: 5000
        })
    }

}