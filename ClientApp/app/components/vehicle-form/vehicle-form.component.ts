import { VehicleService } from '../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";

@Component({
    selector: 'vehicle-form',
    templateUrl: './vehicle-form.component.html'
})
export class VehicleFormComponent implements OnInit {
    makes: any[];
    vehicle: any = {
        features: [],
        contact: {}
    };
    models: any[];
    features: any[];

    constructor(private vehicleService: VehicleService,
                private toastyService: ToastyService) {

    }

    ngOnInit() {
        this.vehicleService.getMakes().subscribe(makes => {
            this.makes = makes;
        });

        this.vehicleService.getFeatures().subscribe(
            features => {
                this.features = features;
        });
    }

    onMakeChange() {
        var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
        this.models = selectedMake ? selectedMake.models : [];
        delete this.vehicle.modelId;
    }

    onFeatureToggle(featureId, $event) {
        if ($event.target.checked)
            this.vehicle.features.push(featureId);
        else {
            var index = this.vehicle.features.indexOf(featureId)
            this.vehicle.features.splice(index, 1);
        }
    }

    submit() {
        this.vehicleService.create(this.vehicle)
            .subscribe(
                x => console.log(x),
                err => {
                    this.toastyService.error({
                        title: 'Error',
                        msg: 'An unexpected error happened.',
                        theme: 'bootstrap',
                        showClose: true,
                        timeout: 5000
                    })
                });
    }
}
