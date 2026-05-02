import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProductService } from '../services/product';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class ProductsComponent implements OnInit {

  products: any[] = [];
  filteredProducts: any[] = [];
  selectedCategory: string = 'all';

  constructor(
    private productService: ProductService,
    private router: Router,
  ) {}

ngOnInit() {
  this.productService.getProducts().subscribe(data => {
    this.products = data;
    this.resetProducts();
  });

  this.router.events.subscribe(() => {
    if (this.router.url === '/') {
      this.resetProducts();
    }
  });
}  

resetProducts() {
  this.selectedCategory = 'all';
  this.filteredProducts = [...this.products];
}

  setCategory(category: string) {
  this.selectedCategory = category;

  if (category === 'all') {
    this.filteredProducts = [...this.products];
    return;
  }

  this.filteredProducts = this.products.filter(p =>
    p.categoryName?.toLowerCase() === category.toLowerCase()
  );
  }

  addToCart(productId: number) {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.productService.addToCart(productId).subscribe({
      next: () => console.log("Added to cart"),
      error: (err) => console.error("Error adding to cart", err)
    });
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}