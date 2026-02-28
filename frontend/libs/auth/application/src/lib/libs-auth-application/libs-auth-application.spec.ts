import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LibsAuthApplication } from './libs-auth-application';

describe('LibsAuthApplication', () => {
  let component: LibsAuthApplication;
  let fixture: ComponentFixture<LibsAuthApplication>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibsAuthApplication],
    }).compileComponents();

    fixture = TestBed.createComponent(LibsAuthApplication);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
