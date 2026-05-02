import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-confirmation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-confirmation.html',
  styleUrls: ['./order-confirmation.css']
})
export class OrderConfirmationComponent {

  order: any;

  constructor(public router: Router) {
    this.order = history.state.order;
  }

  goHome() {
    this.router.navigate(['/']);
  }
}