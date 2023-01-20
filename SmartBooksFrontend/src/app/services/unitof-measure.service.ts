import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UnitofMeasure, UomConversion } from '../models/unitof-measure';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class UnitofMeasureService {

  private controller = 'UnitsofMeasure';
  private dataSourceSubject = new BehaviorSubject<UnitofMeasure[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  private uom = {id: 0, abbreviation: '', description: '', type: 0, uomConversions: []};

  private selectedSubject = new BehaviorSubject<UnitofMeasure>(this.uom);
  public selected = this.selectedSubject.asObservable();

  public wasFetched: boolean;

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    abbreviation:  new FormControl('', Validators.required),
    description:  new FormControl('', Validators.required),
    type: new FormControl(null, Validators.required)
  });

  uomConversionForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    unitofMeasureFromId: new FormControl('', Validators.required),
    unitofMeasureToId: new FormControl('', Validators.required),
    description: new FormControl(''),
    factor: new FormControl('', [
      Validators.required,
      Validators.min(0.1)
    ])
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) { }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        abbreviation:  '',
        description:  '',
        type: 0
      });
    }

    populateFormGroup(model: UnitofMeasure): void {
      this.form.setValue({
        id: model.id,
        abbreviation: model.abbreviation,
        description: model.description,
        type: model.type
      });
    }

    initializeConversionFormGroup(uomFromId: number): void {
      this.uomConversionForm.setValue({
        id: 0,
        unitofMeasureFromId: uomFromId,
        unitofMeasureToId: null,
        description: '',
        factor: 1
      });
    }

    populateConversionFormGroup(model: UomConversion): void {
      this.uomConversionForm.setValue({
        id: model.id,
        unitofMeasureFromId: model.unitofMeasureFromId,
        unitofMeasureToId: model.unitofMeasureToId,
        description: model.description,
        factor: model.factor
      });
    }

    setSelected(selectedUoM: UnitofMeasure): void {
      this.selectedSubject.next(selectedUoM);
    }

    getTypeDescription(type: number): string {
      switch (type) {
        case 0:
          return 'Count';
        case 1:
          return 'Weight';
        case 2:
          return 'Length';
        case 3:
          return 'Area';
        case 4:
          return 'Volume';
        case 5:
          return 'Time';
      }
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<UnitofMeasure[]>(`${this.constants.baseUrl}${this.controller}`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onCreate(model: UnitofMeasure): Observable<UnitofMeasure> {
      return this.http.post<UnitofMeasure>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEdit(model: UnitofMeasure): Observable<UnitofMeasure> {
      return this.http.put<UnitofMeasure>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).abbreviation =  res.abbreviation;
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.value.find(e => e.id === res.id).type =  res.type;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/${id}`)
      .pipe(
        map(res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res);
          if (index > -1) {
            this.dataSourceSubject.value.splice(index, 1);
            if (this.dataSourceSubject.value.length > 0) {
              this.setSelected(this.dataSourceSubject.value[0]);
            } else {
              this.setSelected(this.uom);
            }
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          return res;
        })
      );
    }


    onCreateConversion(model: UomConversion): Observable<UomConversion> {
      return this.http.post<UomConversion>(`${this.constants.baseUrl}${this.controller}/CreateConversion`, model)
      .pipe(
        map(res => {
          this.dataSourceSubject.value.find(e => e.id === res.unitofMeasureFromId)
            .uomConversions.push(res);
          this.dataSourceSubject.value.find(e => e.id === res.unitofMeasureToId)
            .uomConversions.push(res);
          this.dataSourceSubject.next(this.dataSourceSubject.value);
          return res;
        })
      );
    }


    onEditConversion(model: UomConversion): Observable<UomConversion> {
      return this.http.put<UomConversion>(`${this.constants.baseUrl}${this.controller}/UpdateConversion`, model)
      .pipe(
        map(res => {
          this.dataSourceSubject.value
            .find(e => e.id === res.unitofMeasureFromId).uomConversions
            .find(e => e.id === res.id).description =  res.description;
          this.dataSourceSubject.value
            .find(e => e.id === res.unitofMeasureFromId).uomConversions
            .find(e => e.id === res.id).factor =  res.factor;
          this.dataSourceSubject.value
            .find(e => e.id === res.unitofMeasureToId).uomConversions
            .find(e => e.id === res.id).description =  res.description;
          this.dataSourceSubject.value
            .find(e => e.id === res.unitofMeasureToId).uomConversions
            .find(e => e.id === res.id).factor =  res.factor;
          this.dataSourceSubject.next(this.dataSourceSubject.value);
          return res;
        })
      );
    }

    onDeleteConversion(model: UomConversion): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/DeleteConversion/${model.id}`)
      .pipe(
        map(res => {
          this.dataSourceSubject.value
            .find(e => e.id === model.unitofMeasureFromId).uomConversions
            .splice(this.dataSourceSubject.value
              .find(e => e.id === model.unitofMeasureFromId).uomConversions
              .findIndex(e => e.id === res), 1);
          this.dataSourceSubject.value
            .find(e => e.id === model.unitofMeasureToId).uomConversions
            .splice(this.dataSourceSubject.value
              .find(e => e.id === model.unitofMeasureToId).uomConversions
              .findIndex(e => e.id === res), 1);
          this.dataSourceSubject.next(this.dataSourceSubject.value);
          return res;
        })
      );
    }

}
