import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { InventoryItem, InventoryItemsList } from '../models/inventory-item';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  private itemscontroller = 'InventoryItems';
  private controller = 'inventory';

  private inventoryItem = {
    id: 0,
    code: '',
    description: '',
    type: 0,
    unitofMeasureId: null,
    inventoryCategoryId: null,
    taxId: 0,
    standardCost: 0,
    standardPrice: 0,
    assetAcount: null,
    cogsAccount: null,
    incomeAccount: null,
    onHand: 0,
    onOrder: 0,
    allocated: 0,
    backOrdered: 0,
    available: 0,
    totalAverageCost: 0,
    totalStandardCost: 0,
    unitofMeasure: null,
    inventoryCategory: null
  };

  private dataSourceSubject = new BehaviorSubject<InventoryItemsList>({inventoryItems: [], totalCount: 0});
  public dataSource = this.dataSourceSubject.asObservable();

  private selectedItemSubject = new BehaviorSubject<InventoryItem>(this.inventoryItem);
  public selectedItem = this.selectedItemSubject.asObservable();

  private itemCountSubject = new BehaviorSubject<number>(0);
  public itemCount = this.itemCountSubject.asObservable();

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    code:  new FormControl('', Validators.required),
    description:  new FormControl('', Validators.required),
    type: new FormControl('', Validators.required),
    unitofMeasureId: new FormControl('', Validators.required),
    inventoryCategoryId: new FormControl(''),
    taxId: new FormControl(''),
    standardCost: new FormControl(''),
    standardPrice: new FormControl(''),
    assetAcount: new FormControl(''),
    cogsAccount: new FormControl(''),
    incomeAccount: new FormControl('')
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) {}

    public setSelectedItem(inventoryItem: InventoryItem): void {
      this.selectedItemSubject.next(inventoryItem);
    }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        code:  '',
        description:  '',
        type: 0,
        unitofMeasureId: null,
        inventoryCategoryId: null,
        taxId: null,
        standardCost: 0,
        standardPrice: 0,
        assetAcount: null,
        cogsAccount: null,
        incomeAccount: null
      });
    }

    populateFormGroup(model: InventoryItem): void {
      this.form.setValue({
        id: model.id,
        code: model.code,
        description: model.description,
        type: model.type,
        unitofMeasureId: model.unitofMeasureId,
        inventoryCategoryId: model.inventoryCategoryId,
        taxId: model.taxId,
        standardCost: model.standardCost,
        standardPrice: model.standardPrice,
        assetAcount: model.assetAcount,
        cogsAccount: model.cogsAccount,
        incomeAccount: model.incomeAccount
      });
    }

    onGetItems(sort: string, order: string, page: number, pageSize: number): void {
      const requestUrl = `${this.constants.baseUrl}${this.itemscontroller}/${sort}/${order}/${page + 1}/${pageSize}`;
      this.http.get<InventoryItemsList>(requestUrl)
      .subscribe(
        res => {
          this.dataSourceSubject.next(res);
        }
      );
    }

    onCreate(model: InventoryItem): Observable<InventoryItem> {
      return this.http.post<InventoryItem>(`${this.constants.baseUrl}${this.itemscontroller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.inventoryItems.concat([res]);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelectedItem(res);
            return res;
          })
        );
    }

    onEdit(model: InventoryItem): Observable<InventoryItem> {
      return this.http.put<InventoryItem>(`${this.constants.baseUrl}${this.itemscontroller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).code =  res.code;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).inventoryCategoryId =  res.inventoryCategoryId;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).taxId =  res.taxId;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).standardCost =  res.standardCost;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).standardPrice =  res.standardPrice;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).assetAcount =  res.assetAcount;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).cogsAccount =  res.cogsAccount;
            this.dataSourceSubject.value.inventoryItems.find(e => e.id === res.id).incomeAccount =  res.incomeAccount;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelectedItem(res);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.itemscontroller}/${id}`)
        .pipe(
          map(res => {
            const index = this.dataSourceSubject.value.inventoryItems.findIndex(e => e.id === res);
            if (index > -1) {
              this.dataSourceSubject.value.inventoryItems.splice(index, 1);
              this.dataSourceSubject.next(this.dataSourceSubject.value);
            }
            return res;
          })
        );
    }
}
