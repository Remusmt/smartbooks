import { Bin, Warehouse } from './../models/warehouse';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ConstantsService } from '../shared/services/constants.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WarehouseService {

  private dataSourceSubject = new BehaviorSubject<Warehouse[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  private selectedWarehouseSubject: BehaviorSubject<Warehouse>;
  public selectedWarehouse: Observable<Warehouse>;

  public wasFetched: boolean;

  warehouseForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    code:  new FormControl(''),
    description:  new FormControl('', Validators.required),
    defaultReceivingBin: new FormControl(''),
    defaultDespatchBin: new FormControl('')
  });

  binForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    code:  new FormControl(''),
    description:  new FormControl('', Validators.required),
    warehouseId: new FormControl('')
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) {
      const warehouse = {
        id: 0,
        code: '',
        description: '',
        defaultReceivingBin: 0,
        defaultDespatchBin: 0,
        bins: []
      };
      this.selectedWarehouseSubject = new BehaviorSubject<Warehouse>(warehouse);
      this.selectedWarehouse = this.selectedWarehouseSubject.asObservable();
    }

    initializeWarehouseFormGroup(): void {
      this.warehouseForm.setValue({
        id: 0,
        code:  '',
        description:  '',
        defaultReceivingBin: 0,
        defaultDespatchBin: 0
      });
    }

    populateWarehouseFormGroup(warehouse: Warehouse): void {
      this.warehouseForm.setValue({
        id: warehouse.id,
        code: warehouse.code,
        description: warehouse.description,
        defaultReceivingBin: warehouse.defaultReceivingBin,
        defaultDespatchBin: warehouse.defaultDespatchBin
      });
    }

    initializeBinFormGroup(warehouse: number): void {
      this.binForm.setValue({
        id: 0,
        code:  '',
        description:  '',
        warehouseId:  warehouse
      });
    }

    populateBinFormGroup(bin: Bin): void {
      this.binForm.setValue({
        id: bin.id,
        code:  bin.code,
        description:  bin.description,
        warehouseId:  bin.warehouseId
      });
    }

    public setSelectedWarehouse(warehouse: Warehouse): void {
      this.selectedWarehouseSubject.next(warehouse);
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<Warehouse[]>(`${this.constants.baseUrl}warehouse`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onCreate(model: Warehouse): Observable<Warehouse> {
      return this.http.post<Warehouse>(`${this.constants.baseUrl}warehouse`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEdit(model: Warehouse): Observable<Warehouse> {
      return this.http.put<Warehouse>(`${this.constants.baseUrl}warehouse`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).id = res.id;
            this.dataSourceSubject.value.find(e => e.id === res.id).code =  res.code;
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.value.find(e => e.id === res.id).defaultDespatchBin =  res.defaultDespatchBin;
            this.dataSourceSubject.value.find(e => e.id === res.id).defaultReceivingBin =  res.defaultReceivingBin;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}warehouse/${id}`)
      .pipe(
        map(res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res);
          if (index > -1) {
            this.dataSourceSubject.value.splice(index, 1);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          return res;
        })
      );
    }

    onCreateBin(model: Bin): Observable<Bin> {
      return this.http.post<Bin>(`${this.constants.baseUrl}bins`, model)
        .pipe(
          map(res => {
            const bin: Bin = {
              id: res.id,
              code: res.code,
              description: res.description,
              warehouseId: res.warehouseId
            };
            this.selectedWarehouseSubject.value.bins.push(bin);
            this.selectedWarehouseSubject.next(this.selectedWarehouseSubject.value);
            return res;
          })
        );
    }

    onEditBin(model: Bin): Observable<Bin> {
      return this.http.put<Bin>(`${this.constants.baseUrl}bins`, model)
        .pipe(
          map(res => {
            this.selectedWarehouseSubject.value.bins.find(e => e.id === res.id).code = res.code;
            this.selectedWarehouseSubject.value.bins.find(e => e.id === res.id).description =  res.description;
            this.selectedWarehouseSubject.next(this.selectedWarehouseSubject.value);
            return res;
          })
        );
    }

    onDeleteBin(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}bins/${id}`)
      .pipe(
        map(res => {
          const index = this.selectedWarehouseSubject.value.bins.findIndex(e => e.id === res);
          if (index > -1) {
            this.selectedWarehouseSubject.value.bins.splice(index, 1);
            this.selectedWarehouseSubject.next(this.selectedWarehouseSubject.value);
          }
          return res;
        })
      );
    }

}
