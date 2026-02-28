import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LibsAuthDomain } from './libs-auth-domain';

describe('LibsAuthDomain', () => {
  let component: LibsAuthDomain;
  let fixture: ComponentFixture<LibsAuthDomain>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibsAuthDomain],
    }).compileComponents();

    fixture = TestBed.createComponent(LibsAuthDomain);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
