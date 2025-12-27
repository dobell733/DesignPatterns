using Observer;

Console.Title = "Observer";

// create observers
TicketStockService ticketStockService = new();
TicketResellerService ticketResellerService = new();
// create subject
OrderService orderService = new();

// add two observers
orderService.AddObserver(ticketStockService);
orderService.AddObserver(ticketResellerService);

// notify
orderService.CompleteTicketSale(1, 2);

Console.WriteLine();

// remove one observer
orderService.RemoveObserver(ticketResellerService);

// notify
orderService.CompleteTicketSale(2, 4);

Console.ReadKey();