import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { UnitofMeasure } from 'src/app/models/unitof-measure';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-edit-uom-conversion',
  templateUrl: './edit-uom-conversion.component.html',
  styleUrls: ['./edit-uom-conversion.component.css']
})
export class EditUomConversionComponent implements OnInit, OnDestroy {

  saving = false;
  unitofMeasure: UnitofMeasure;
  unitTo: UnitofMeasure;
  description = '';
  unitsofMeasure: UnitofMeasure[] = [];

  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: UnitofMeasureService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<EditUomConversionComponent>) { }

  ngOnInit(): void {
    this.unitTo = {id: 0, abbreviation: '', description: '', type: 0, uomConversions: []};
    this.unitofMeasure = {id: 0, abbreviation: '', description: '', type: 0, uomConversions: []};

    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.unitsofMeasure = res;
          this.unitTo = this.unitsofMeasure
              .find(e => e.id === this.service.uomConversionForm.get('unitofMeasureToId').value);
          this.unitofMeasure = this.unitsofMeasure
              .find(e => e.id === this.service.uomConversionForm.get('unitofMeasureFromId').value);
          this.getDescription();
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onClose(): void {
    this.service.uomConversionForm.reset();
    this.dialogRef.close();
  }

  onFactorChange(): void {
    this.getDescription();
  }

  private getDescription(): void {
    this.description = `1 ${this.unitofMeasure.abbreviation} = ${this.service.uomConversionForm.get('factor').value} ${this.unitTo.abbreviation}`;
    this.service.uomConversionForm.controls.description.setValue(this.description);
  }

  onSubmit(): void {
    if (this.service.uomConversionForm.valid) {
      this.subscriptions.add(
        this.service.onEditConversion(this.service.uomConversionForm.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Update successfully');
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
}
