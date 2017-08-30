import { Vehicle } from '../../models/vehicle';
import { SaveVehicle } from '../../models/saveVehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/Observable/forkJoin';
@Component({
    selector: 'vehicle-form',
    templateUrl: './vehicle-form.component.html'
})
export class VehicleFormComponent implements OnInit {
    makes: any[];
    vehicle: SaveVehicle = {
        id: null,
        makeId: undefined,
        modelId: undefined,
        isRegistered: false,
        features: [],
        contact: {
            name: '',
            email: '',
            phone: ''
        }
    };
    models: any[];
    features: any[];

    constructor(private vehicleService: VehicleService,
        private router: Router,
        private route: ActivatedRoute,
        private toastyService: ToastyService) {
        route.params.subscribe(p => {
            this.vehicle.id = +p['id'];
        });
    }

    ngOnInit() {
        var sources = [
            this.vehicleService.getMakes(),
            this.vehicleService.getFeatures(),
        ];

        if (this.vehicle.id) {
            sources.push(
                this.vehicleService.getVehicle(this.vehicle.id))
        }

        Observable.forkJoin(sources).subscribe(data => {
            this.makes = data[0];
            this.features = data[1];
            if (this.vehicle.id) {
                this.setVehicle(data[2]);
            }
        }, err => {
            if (err.status == 404) {
                this.router.navigate(['/home']);
            }
        });

    }

    private setVehicle(v: Vehicle) {
        this.vehicle.id = v.id;
        this.vehicle.makeId = v.make.id;
        this.vehicle.modelId = v.model.id;
        this.vehicle.contact = v.contact;
        this.vehicle.isRegistered = v.isRegistered;
        this.vehicle.features = v.features.map(f => f.id);
        this.populateModels();
    }

    onMakeChange() {
        this.populateModels();
        delete this.vehicle.modelId;
    }

    private populateModels() {
        var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
        this.models = selectedMake ? selectedMake.models : [];
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
        if (this.vehicle.id > 0) {
            this.vehicleService.update(this.vehicle)
                .subscribe(x => {
                    this.toastyService.success({
                        title: 'Success!',
                        msg: 'The vehicle was successfully updated',
                        theme: 'bootstrap',
                        showClose: true,
                        timeout: 5000
                    });
                });
        } else {
            this.vehicle.id = 0;
            this.vehicleService.create(this.vehicle)
                .subscribe(x => {
                    this.toastyService.success({
                        title: 'Success!',
                        msg: 'The vehicle was successfully created',
                        theme: 'bootstrap',
                        showClose: true,
                        timeout: 5000
                    });
                });
        }
    }

    delete() {
        if (confirm("Are you sure?")) {
            this.vehicleService.delete(this.vehicle.id)
                .subscribe(x => {
                    this.toastyService.success({
                        title: 'Success!',
                        msg: 'The vehicle was successfully deleted',
                        theme: 'bootstrap',
                        showClose: true,
                        timeout: 5000
                    });
                    this.router.navigate(['/home'])
                });
        }
    }
}
