import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LibsAuthInfrastructure } from './libs-auth-infrastructure';

describe('LibsAuthInfrastructure', () => {
  let component: LibsAuthInfrastructure;
  let fixture: ComponentFixture<LibsAuthInfrastructure>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibsAuthInfrastructure],
    }).compileComponents();

    fixture = TestBed.createComponent(LibsAuthInfrastructure);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
