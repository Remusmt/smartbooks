import { TaxDetailsComponent } from './views/taxes/tax-details/tax-details.component';
import { TaxesMainComponent } from './views/taxes/taxes-main/taxes-main.component';
import { SupplierDetailsComponent } from './views/suppliers/supplier-details/supplier-details.component';
import { CustomerDetailsComponent } from './views/customers/customer-details/customer-details.component';
import { CustomersMainComponent } from './views/customers/customers-main/customers-main.component';
import { InventoryItemDetailsComponent } from './views/inventory/inventory-item-details/inventory-item-details.component';
import { InventoryMainComponent } from './views/inventory/inventory-main/inventory-main.component';
import { ChartofAccountMainComponent } from './views/chartsofaccount/chartof-account-main/chartof-account-main.component';
import { PaymentTermsMainComponent } from './views/paymentterms/payment-terms-main/payment-terms-main.component';
import { InventoryCategoriesMainComponent } from './views/inventory/inventory-categories-main/inventory-categories-main.component';
import { SupplierTypesMainComponent } from './views/suppliers/supplier-types-main/supplier-types-main.component';
import { CustomerTypeMainComponent } from './views/customers/customer-type-main/customer-type-main.component';
import { CostcentersMainComponent } from './views/costcenters/costcenters-main/costcenters-main.component';
import { UsersMainComponent } from './views/users/users-main/users-main.component';
import { WarehouseMainComponent } from './views/warehouse/warehouse-main/warehouse-main.component';
import { HomeComponent } from './views/home/home.component';
import { LoginComponent } from './views/login/login.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './shared/services/auth.guard';
import { UnitsofMeasureMainComponent } from './views/unitsofmeasure/unitsof-measure-main/unitsof-measure-main.component';
import { SuppliersMainComponent } from './views/suppliers/suppliers-main/suppliers-main.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'app', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      {path: '', component: TaxesMainComponent},
      {path: 'warehouses', component: WarehouseMainComponent},
      {path: 'users', component: UsersMainComponent},
      {path: 'costcenters', component: CostcentersMainComponent},
      {path: 'customertypes', component: CustomerTypeMainComponent},
      {path: 'suppliertypes', component: SupplierTypesMainComponent},
      {path: 'inventorycategories', component: InventoryCategoriesMainComponent},
      {path: 'paymentterms', component: PaymentTermsMainComponent},
      {path: 'chartsofaccount', component: ChartofAccountMainComponent},
      {path: 'inventory', component: InventoryMainComponent},
      {path: 'itemdetails', component: InventoryItemDetailsComponent},
      {path: 'uoms', component: UnitsofMeasureMainComponent},
      {path: 'sales', component: CustomersMainComponent},
      {path: 'customerdetails', component: CustomerDetailsComponent},
      {path: 'purchases', component: SuppliersMainComponent},
      {path: 'supplierdetails', component: SupplierDetailsComponent},
      {path: 'taxes', component: TaxesMainComponent},
      {path: 'taxdetails', component: TaxDetailsComponent}
    ]},
  {path: '', redirectTo: '/app', pathMatch: 'full'},
  // {path: '**', component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
