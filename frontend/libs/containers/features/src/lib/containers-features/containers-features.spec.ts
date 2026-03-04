import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContainersFeatures } from './containers-features';

describe('ContainersFeatures', () => {
  let component: ContainersFeatures;
  let fixture: ComponentFixture<ContainersFeatures>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainersFeatures],
    }).compileComponents();

    fixture = TestBed.createComponent(ContainersFeatures);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
