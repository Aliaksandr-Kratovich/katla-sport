export class Product {
    constructor(
        public id: number,
        public code: string,
        public name: string,
        public categoryId: number,
        public description: string,
        public manufacturerCode: string,
        public price: number,
        public isDeleted: boolean,
        public lastUpdated: string
    ) { }
}
