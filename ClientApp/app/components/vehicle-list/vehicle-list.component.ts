import { KeyValuePair } from '../../models/keyValuePair';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle } from '../../models/vehicle';
import { Component, OnInit } from '@angular/core';
import { Observable } from "rxjs/Observable";

@Component({
    selector: 'vehicle-list',
    templateUrl: 'vehicle-list.component.html'
})

export class VehicleListComponent implements OnInit {
    vehicles: Vehicle[];
    makes: any[];
    models: KeyValuePair[];
    filter: any = {};

    constructor(private vehicleService: VehicleService) { }

    ngOnInit() {
        var sources = [
            this.vehicleService.getMakes(),
            this.vehicleService.getVehicles(this.filter)
        ];

        Observable.forkJoin(sources).subscribe(data => {
            this.makes = data[0];
            this.vehicles = data[1];
        });
    }

    onFilterChange() {
        this.vehicleService.getVehicles(this.filter)
            .subscribe(vehicles => this.vehicles = vehicles);
    }

    resetFilter() {
        this.filter = {};
        this.onFilterChange();
    }
}