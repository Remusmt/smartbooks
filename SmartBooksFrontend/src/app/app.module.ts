import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { MaterialModule } from './shared/modules/material/material.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { AccessDeniedComponent } from './shared/access-denied/access-denied.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { JwtInterceptor } from './shared/services/jwt.interceptor';
import { LoginComponent } from './views/login/login.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HomeComponent } from './views/home/home.component';
import { WarehouseListComponent } from './views/warehouse/warehouse-list/warehouse-list.component';
import { WarehouseMainComponent } from './views/warehouse/warehouse-main/warehouse-main.component';
import { WarehouseDetailsComponent } from './views/warehouse/warehouse-details/warehouse-details.component';
import { WarehouseComponent } from './views/warehouse/warehouse/warehouse.component';
import { BinComponent } from './views/warehouse/bin/bin.component';
import { BinListComponent } from './views/warehouse/bin-list/bin-list.component';
import { UsersMainComponent } from './views/users/users-main/users-main.component';
import { UsersListComponent } from './views/users/users-list/users-list.component';
import { UsersDetailsComponent } from './views/users/users-details/users-details.component';
import { UsersRightsListComponent } from './views/users/users-rights-list/users-rights-list.component';
import { CreateUserComponent } from './views/users/create-user/create-user.component';
import { EditUserComponent } from './views/users/edit-user/edit-user.component';
import { CostcentersMainComponent } from './views/costcenters/costcenters-main/costcenters-main.component';
import { CostcenterComponent } from './views/costcenters/costcenter/costcenter.component';
import { CustomerTypeMainComponent } from './views/customers/customer-type-main/customer-type-main.component';
import { CustomerTypeComponent } from './views/customers/customer-type/customer-type.component';
import { InventoryCategoriesMainComponent } from './views/inventory/inventory-categories-main/inventory-categories-main.component';
import { InventoryCategoryComponent } from './views/inventory/inventory-category/inventory-category.component';
import { SupplierTypesMainComponent } from './views/suppliers/supplier-types-main/supplier-types-main.component';
import { SupplierTypeComponent } from './views/suppliers/supplier-type/supplier-type.component';
import { PaymentTermsMainComponent } from './views/paymentterms/payment-terms-main/payment-terms-main.component';
import { PaymentTermsComponent } from './views/paymentterms/payment-terms/payment-terms.component';
import { ChartofAccountMainComponent } from './views/chartsofaccount/chartof-account-main/chartof-account-main.component';
import { ChartofAccountComponent } from './views/chartsofaccount/chartof-account/chartof-account.component';
import { InventoryMainComponent } from './views/inventory/inventory-main/inventory-main.component';
import { InventoryItemsListComponent } from './views/inventory/inventory-items-list/inventory-items-list.component';
import { InventoryItemDetailsComponent } from './views/inventory/inventory-item-details/inventory-item-details.component';
import { InventoryItemComponent } from './views/inventory/inventory-item/inventory-item.component';
import { UnitsofMeasureMainComponent } from './views/unitsofmeasure/unitsof-measure-main/unitsof-measure-main.component';
import { UnitofMeasureComponent } from './views/unitsofmeasure/unitof-measure/unitof-measure.component';
import { UnitsofMeasureListComponent } from './views/unitsofmeasure/unitsof-measure-list/unitsof-measure-list.component';
import { UnitsofMeasureDetailsComponent } from './views/unitsofmeasure/unitsof-measure-details/unitsof-measure-details.component';
import { UomConversionsListComponent } from './views/unitsofmeasure/uom-conversions-list/uom-conversions-list.component';
import { UomConversionComponent } from './views/unitsofmeasure/uom-conversion/uom-conversion.component';
import { EditUomConversionComponent } from './views/unitsofmeasure/edit-uom-conversion/edit-uom-conversion.component';
import { CustomersMainComponent } from './views/customers/customers-main/customers-main.component';
import { CustomersListComponent } from './views/customers/customers-list/customers-list.component';
import { CustomerDetailsComponent } from './views/customers/customer-details/customer-details.component';
import { CustomerComponent } from './views/customers/customer/customer.component';
import { CustomerAddressesListComponent } from './views/customers/customer-addresses-list/customer-addresses-list.component';
import { CustomerAddressComponent } from './views/customers/customer-address/customer-address.component';
import { SuppliersMainComponent } from './views/suppliers/suppliers-main/suppliers-main.component';
import { SupplierComponent } from './views/suppliers/supplier/supplier.component';
import { SupplierAddressComponent } from './views/suppliers/supplier-address/supplier-address.component';
import { SupplierAddressesListComponent } from './views/suppliers/supplier-addresses-list/supplier-addresses-list.component';
import { SupplierDetailsComponent } from './views/suppliers/supplier-details/supplier-details.component';
import { SuppliersListComponent } from './views/suppliers/suppliers-list/suppliers-list.component';
import { TaxesMainComponent } from './views/taxes/taxes-main/taxes-main.component';
import { TaxesListComponent } from './views/taxes/taxes-list/taxes-list.component';
import { TaxComponent } from './views/taxes/tax/tax.component';
import { TaxDetailsComponent } from './views/taxes/tax-details/tax-details.component';
import { TaxRateComponent } from './views/taxes/tax-rate/tax-rate.component';
import { TaxRatesListComponent } from './views/taxes/tax-rates-list/tax-rates-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    AccessDeniedComponent,
    NotFoundComponent,
    SpinnerComponent,
    LoginComponent,
    HomeComponent,
    WarehouseListComponent,
    WarehouseMainComponent,
    WarehouseDetailsComponent,
    WarehouseComponent,
    BinComponent,
    BinListComponent,
    UsersMainComponent,
    UsersListComponent,
    UsersDetailsComponent,
    UsersRightsListComponent,
    CreateUserComponent,
    EditUserComponent,
    CostcentersMainComponent,
    CostcenterComponent,
    CustomerTypeMainComponent,
    CustomerTypeComponent,
    InventoryCategoriesMainComponent,
    InventoryCategoryComponent,
    SupplierTypesMainComponent,
    SupplierTypeComponent,
    PaymentTermsMainComponent,
    PaymentTermsComponent,
    ChartofAccountMainComponent,
    ChartofAccountComponent,
    InventoryMainComponent,
    InventoryItemsListComponent,
    InventoryItemDetailsComponent,
    InventoryItemComponent,
    UnitsofMeasureMainComponent,
    UnitofMeasureComponent,
    UnitsofMeasureListComponent,
    UnitsofMeasureDetailsComponent,
    UomConversionsListComponent,
    UomConversionComponent,
    EditUomConversionComponent,
    CustomersMainComponent,
    CustomersListComponent,
    CustomerDetailsComponent,
    CustomerComponent,
    CustomerAddressesListComponent,
    CustomerAddressComponent,
    SuppliersMainComponent,
    SupplierComponent,
    SupplierAddressComponent,
    SupplierAddressesListComponent,
    SupplierDetailsComponent,
    SuppliersListComponent,
    TaxesMainComponent,
    TaxesListComponent,
    TaxComponent,
    TaxDetailsComponent,
    TaxRateComponent,
    TaxRatesListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
