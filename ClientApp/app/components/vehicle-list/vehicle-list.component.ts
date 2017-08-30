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
    allVehicles: Vehicle[];
    makes: any[];
    models: KeyValuePair[];
    filter: any = { };

    constructor(private vehicleService: VehicleService) { }

    ngOnInit() {
        var sources = [
            this.vehicleService.getMakes(),
            this.vehicleService.getVehicles()
        ];

        Observable.forkJoin(sources).subscribe(data => {
            this.makes = data[0];
            this.vehicles = this.allVehicles = data[1];
        });
    }

    onFilterChange() {
        var vehicles = this.allVehicles;

        if (this.filter.makeId) {
            vehicles = vehicles.filter(v => v.make.id == this.filter.makeId);
            var selectedMake = this.makes.find(m => m.id == this.filter.makeId);
            this.models = selectedMake ? selectedMake.models : [];
        }

        if (this.filter.modelId) 
            vehicles = vehicles.filter(v => v.model.id == this.filter.modelId);

        if (this.filter.isRegistered)
            vehicles = vehicles.filter(v => v.isRegistered == <any>this.filter.isRegistered)

        this.vehicles = vehicles;
    }

    resetFilter() {
        this.filter = {};
        this.onFilterChange();
    }
}