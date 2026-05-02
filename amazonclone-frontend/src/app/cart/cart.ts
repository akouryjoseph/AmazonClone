import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class CartComponent implements OnInit {

  cart: any;

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
  this.http.get('http://localhost:5238/api/Cart')
    .subscribe(data => {
      this.cart = data;
      this.cdr.detectChanges();
    });
}

removeItem(productId: number, ) {

    this.http.delete(`http://localhost:5238/api/Cart/remove/${productId}`)
      .subscribe(() => {
        this.loadCart();
        this.cdr.detectChanges(); 
      });
  }


  getTotal(): number {
    if (!this.cart || !this.cart.items) return 0;

    return this.cart.items.reduce((sum: number, item: any) => {
      return sum + (item.product.price * item.quantity);
    }, 0);
  }

  increaseQuantity(item: any) {
  this.http.post(`http://localhost:5238/api/cart/add/${item.product.id}`, {})
    .subscribe(() => {
      this.loadCart(); // in order to always refresh
    });
}

  decreaseQuantity(item: any) {
  this.http.post(`http://localhost:5238/api/cart/decrease/${item.product.id}`, {})
    .subscribe(() => {
      this.loadCart();
    });
}

  checkout() {
  this.router.navigate(['/checkout']);
}
}