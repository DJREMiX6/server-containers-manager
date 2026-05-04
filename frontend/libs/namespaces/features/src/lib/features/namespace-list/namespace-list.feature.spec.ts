import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NamespaceListFeature } from './namespace-list.feature';

describe('NamespaceListFeature', () => {
  let component: NamespaceListFeature;
  let fixture: ComponentFixture<NamespaceListFeature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NamespaceListFeature],
    }).compileComponents();

    fixture = TestBed.createComponent(NamespaceListFeature);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
