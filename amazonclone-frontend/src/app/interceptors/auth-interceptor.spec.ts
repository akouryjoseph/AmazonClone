import { TestBed } from '@angular/core/testing';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AuthInterceptor } from './auth-interceptor';

describe('AuthInterceptor', () => {

  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthInterceptor,
          multi: true
        }
      ]
    });

    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should add Authorization header if token exists', () => {
    localStorage.setItem('token', 'test-token');

    const http = TestBed.inject(HttpClient);

    http.get('/test').subscribe();

    const req = httpMock.expectOne('/test');

    expect(req.request.headers.has('Authorization')).toBe(true);
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');

    req.flush({});
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

});