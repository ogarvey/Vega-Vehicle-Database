import { VehicleService } from '../../services/vehicle.service';
import { Vehicle } from '../../models/vehicle';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'vehicle-list',
    templateUrl: 'vehicle-list.component.html'
})

export class VehicleListComponent implements OnInit {
    vehicles: Vehicle[];
    
    constructor(private vehicleService: VehicleService) { }

    ngOnInit() { 
        this.vehicleService.getVehicles()
            .subscribe(vehicles => this.vehicles = vehicles);
    }
}