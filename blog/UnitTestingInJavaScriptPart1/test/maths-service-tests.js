const MathsService = require('../maths-service.js');
const chai = require('chai');
const expect = chai.expect; 

describe('Factorial', function() {
    it('factorial should return correct value', function() {
        const mathsService = new MathsService();
    
        expect(mathsService.factorial(-1)).to.equal(-1);
        expect(mathsService.factorial(0)).to.equal(1);
        expect(mathsService.factorial(1)).to.equal(1);
        expect(mathsService.factorial(3)).to.equal(6);
        expect(mathsService.factorial(5)).to.equal(120);
    });
});
