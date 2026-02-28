import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LibsAuthFeatures } from './libs-auth-features';

describe('LibsAuthFeatures', () => {
  let component: LibsAuthFeatures;
  let fixture: ComponentFixture<LibsAuthFeatures>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibsAuthFeatures],
    }).compileComponents();

    fixture = TestBed.createComponent(LibsAuthFeatures);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
