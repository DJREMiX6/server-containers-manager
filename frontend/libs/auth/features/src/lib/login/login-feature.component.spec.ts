import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginFeatureComponent } from './login-feature.component';

describe('LoginFeatureComponent', () => {
  let component: LoginFeatureComponent;
  let fixture: ComponentFixture<LoginFeatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginFeatureComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginFeatureComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
