import { AppErrorHandler } from './app.error-handler';
import { VehicleService } from './services/vehicle.service';
import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { sharedConfig } from './app.module.shared';

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        ...sharedConfig.imports
    ],
    providers: [
        {
            provide: ErrorHandler,
            useClass: AppErrorHandler
        },
        VehicleService,
        { provide: 'ORIGIN_URL', useValue: location.origin },
    ]
})
export class AppModule {
}
