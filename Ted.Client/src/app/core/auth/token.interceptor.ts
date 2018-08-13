import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpErrorResponse
} from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

import { AuthService } from './services/auth.service';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(public authService: AuthService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${this.authService.getToken()}`
            }
        });

        return next.handle(request)
            .pipe(
                tap(
                    (event: HttpEvent<any>) => {
                        if (event instanceof HttpResponse) {

                        }
                    }, (err: any) => {
                        if (err instanceof HttpErrorResponse) {
                            if (err.status === 401) {
                                this.authService.logout();
                            }
                        }
                    }));
    }
}
