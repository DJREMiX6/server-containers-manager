import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CopyTextField } from './copy-text-field';

describe('CopyTextField', () => {
  let component: CopyTextField;
  let fixture: ComponentFixture<CopyTextField>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CopyTextField],
    }).compileComponents();

    fixture = TestBed.createComponent(CopyTextField);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
