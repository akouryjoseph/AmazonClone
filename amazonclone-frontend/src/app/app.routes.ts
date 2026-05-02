import { Routes } from '@angular/router';
import { ProductsComponent } from './products/products';
import { CartComponent } from './cart/cart';
import { LoginComponent } from './login/login';
import { CheckoutComponent } from './checkout/checkout';
import { OrderConfirmationComponent } from './order-confirmation/order-confirmation';

export const routes: Routes = [
  { path: '', component: ProductsComponent },
  { path: 'cart', component: CartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'order-confirmation', component: OrderConfirmationComponent }
];