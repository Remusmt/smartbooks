import { UnitofMeasure } from './../../../models/unitof-measure';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-uom-conversion',
  templateUrl: './uom-conversion.component.html',
  styleUrls: ['./uom-conversion.component.css']
})
export class UomConversionComponent implements OnInit, OnDestroy {

  saving = false;
  selectedIsGreater = true;
  private subscriptions: Subscription = new Subscription();

  unitofMeasure: UnitofMeasure;
  unitTo: UnitofMeasure;
  description = '';

  unitsofMeasure: UnitofMeasure[] = [];

  constructor(
    public service: UnitofMeasureService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<UomConversionComponent>) { }


  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnInit(): void {
    this.unitTo = {id: 0, abbreviation: '', description: '', type: 0, uomConversions: []};
    this.subscriptions.add(
      this.service.selected.subscribe(
        (res: UnitofMeasure) => {
          this.unitofMeasure = res;
        }
      )
    );
    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.unitsofMeasure = res;
        }
      )
    );
  }

  onSubmit(): void {
    if (this.service.uomConversionForm.valid) {
      this.saving = true;
      if (this.selectedIsGreater) {
        // then uom to is from the smaller one should be to
        this.service.uomConversionForm.controls.unitofMeasureFromId.setValue(this.unitTo.id);
        this.service.uomConversionForm.controls.unitofMeasureToId.setValue(this.unitofMeasure.id);
      } else {
        this.service.uomConversionForm.controls.unitofMeasureFromId.setValue(this.unitofMeasure.id);
        this.service.uomConversionForm.controls.unitofMeasureToId.setValue(this.unitTo.id);
      }
      this.subscriptions.add(
        this.service.onCreateConversion(this.service.uomConversionForm.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Saved successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err);
              this.saving = false;
            }
      ));
    }
  }

  onClose(): void {
    this.service.uomConversionForm.reset();
    this.dialogRef.close();
  }

  getUomsByType(): UnitofMeasure[] {
    return this.unitsofMeasure
    .filter(e => e.type === this.unitofMeasure.type
      && e.id !== this.unitofMeasure.id);
  }

  onSelectedValueChange(event): void {
    this.unitTo = this.unitsofMeasure.find(e => e.id === event.source.value);
    if (this.unitTo === undefined) {
      this.unitTo = {id: 0, abbreviation: '', description: '', type: 0, uomConversions: []};
    }
    this.getDescription();
  }

  onFactorChange(): void {
    this.getDescription();
  }

  onGreaterChanged(): void {
    this.selectedIsGreater = !this.selectedIsGreater;
    this.getDescription();
  }

  private getDescription(): void {
    if (this.selectedIsGreater) {
      this.description = `1 ${this.unitofMeasure.abbreviation} = ${this.service.uomConversionForm.get('factor').value} ${this.unitTo.abbreviation}`;
    } else {
      this.description = `1 ${this.unitTo.abbreviation} = ${this.service.uomConversionForm.get('factor').value} ${this.unitofMeasure.abbreviation}`;
    }
    this.service.uomConversionForm.controls.description.setValue(this.description);
  }

}
