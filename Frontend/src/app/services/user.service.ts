import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environments';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private usernameSubject = new BehaviorSubject<string>('');
  username$ = this.usernameSubject.asObservable();

  constructor(private http: HttpClient) {
    if (localStorage.getItem('Template_email')) {
      this.isAuthenticatedSubject.next(true);
    }
  }

  setIsAuthenticated(isAuthenticated: boolean, email?: string) {
    if (email) {
      localStorage.setItem('Template_email', email);
      this.usernameSubject.next(email);
    } else {
      localStorage.removeItem('Template_email');
      this.usernameSubject.next('');
    }
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  setJwtToken(token: any) {
    localStorage.setItem('Template_jwt', token.accessToken);
    localStorage.setItem('Template_refreshToken', token.refreshToken);
  }

  clearJwtToken() {
    localStorage.removeItem('Template_jwt');
    localStorage.removeItem('Template_refreshToken');
  }

  login(dto: any): any {
    return this.http.post(
      environment.apiUrl + '/ApplicationUsers/LoginUser',
      dto
    );
  }

  logout(): any {
    return this.http.post(
      environment.apiUrl +
        '/ApplicationUsers/LogoutUser/' +
        localStorage.getItem('Template_email'),
      {}
    );
  }

  getByEmail(email: string): any {
    return this.http.get(
      environment.apiUrl + '/ApplicationUsers/GetByEmail/' + email
    );
  }

  register(dto: any): any {
    return this.http.post(
      environment.apiUrl + '/ApplicationUsers/RegisterUser',
      dto
    );
  }

  getRefreshToken(userRefreshToken: any) {
    return this.http.post(
      environment.apiUrl + 'ApplicationUsers/RefreshToken',
      userRefreshToken
    );
  }

  editUser(dto: any): any {
    return this.http.put(
      environment.apiUrl + '/ApplicationUsers/EditUser',
      dto
    );
  }
}
