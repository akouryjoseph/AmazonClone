import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './checkout.html',
  styleUrls: ['./checkout.css']
})
export class CheckoutComponent implements OnInit {

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  step = 1;
  cart: any = null;
  total = 0;
  checkoutData: any = null;

  address = {
    name: '',
    line1: '',
    city: '',
    zip: ''
  };

  delivery: string = 'standard';

  payment = {
    name: '',
    number: '',
    expiry: '',
    cvv: ''
  };

  ngOnInit() {
  this.loadCart();
}

goToCart() {
  this.router.navigate(['/cart']);
}

loadCart() {
  this.http.get<any>('http://localhost:5238/api/cart')
    .subscribe(data => {

      this.cart = data;

      // create SINGLE source of truth
      this.checkoutData = {
        items: data.items,
        total: this.calculateTotalFromCart(data.items)
      };

      this.total = this.checkoutData.total;
    });
}

calculateTotalFromCart(items: any[]): number {
  if (!items) return 0;

  return items.reduce((sum, item) => {
    return sum + item.product.price * item.quantity;
  }, 0);
}

nextStep() {
  this.step++;
}

prevStep() {
  this.step--;
}

placeOrder() {
  this.http.post('http://localhost:5238/api/Orders/checkout', this.checkoutData)
    .subscribe((res: any) => {

      this.router.navigate(['/order-confirmation'], {
        state: {
          order: {
            items: this.checkoutData.items,
            total: this.checkoutData.total,
            orderId: res?.orderId
          }
        }
      });

    });
}

}