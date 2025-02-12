using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    // UI Elements
    private VisualElement root;
    private VisualElement itemGrid;
    private Label itemNameLabel;
    private Label itemDescriptionLabel;
    private bool isInventoryOpen = false;

    // Array to hold 16 UI item slots (assuming your UXML defines these slots)
    private Button[] itemSlots = new Button[16];

    // Assign your ScriptableObject items in the Inspector (base items)
    [Header("Item Database (Assign Scriptable Objects)")]
    [SerializeField] private InventoryItemData[] itemDatabase;

    // The runtime instances created from your item database
    private InventoryItemInstance[] items;

    void OnEnable()
    {
        // Get the UIDocument and its root VisualElement
        var document = GetComponent<UIDocument>();
        if (document == null)
        {
            Debug.LogError("UIDocument is missing from the GameObject!");
            return;
        }

        root = document.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("Root VisualElement is missing!");
            return;
        }

        // Query required UI elements (ensure these names match your UXML)
        itemGrid = root.Q<VisualElement>("ItemGrid");
        itemNameLabel = root.Q<Label>("ItemNameLabel");
        itemDescriptionLabel = root.Q<Label>("ItemDescriptionLabel");

        if (itemGrid == null || itemNameLabel == null || itemDescriptionLabel == null)
        {
            Debug.LogError("One or more UI elements are missing!");
            return;
        }

        // If no items are provided in the database, initialize an empty array
        if (itemDatabase == null || itemDatabase.Length == 0)
        {
            items = new InventoryItemInstance[16];  // Initialize empty inventory with 16 slots
            Debug.Log("No items in the item database. Initializing empty inventory.");
        }
        else
        {
            // Create runtime instances from the item database if it has items
            items = new InventoryItemInstance[itemDatabase.Length];
            for (int i = 0; i < itemDatabase.Length; i++)
            {
                items[i] = new InventoryItemInstance(itemDatabase[i], itemDatabase[i].defaultQuantity);
            }
        }

        InitializeSlots(); // Initialize the 16 UI slots

        // Initially hide the inventory UI
        root.style.display = DisplayStyle.None;
        Debug.Log("Inventory initialized and hidden.");
    }

    void Update()
    {
        // Toggle inventory on/off with the I key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Initialize 16 UI slots (make sure your UXML contains elements named "ItemSlot_1" to "ItemSlot_16")
    void InitializeSlots()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = itemGrid.Q<Button>($"ItemSlot_{i + 1}");
            if (itemSlots[i] == null)
            {
                Debug.LogError($"Item slot {i + 1} is missing in the UXML!");
                continue;
            }

            int index = i; // Capture index for callback
            itemSlots[i].RegisterCallback<ClickEvent>(evt =>
            {
                if (evt.clickCount == 2) // Double-click to use the item
                {
                    if (index < items.Length && items[index] != null)
                    {
                        UseItem(items[index]);
                    }
                }
            });

            Debug.Log($"Item slot {i + 1} initialized successfully.");
        }
    }

    // Populate the inventory UI with runtime item instances
    void PopulateInventory()
    {
        for (int i = 0; i < items.Length && i < itemSlots.Length; i++)
        {
            var itemInstance = items[i];
            var itemSlot = itemSlots[i];

            if (itemInstance == null)
            {
                Debug.LogError($"Item instance at index {i} is missing!");
                continue;
            }

            // Clear previous children to avoid duplicate icons
            itemSlot.Clear();

            // Create an Image element for the item icon
            var itemIcon = new Image();
            itemIcon.sprite = itemInstance.itemData.icon != null ? itemInstance.itemData.icon : LoadIcon("missing");
            itemSlot.Add(itemIcon);

            // Set the slot text to display the item name and current quantity
            itemSlot.text = $"{itemInstance.itemData.itemName} x{itemInstance.quantity}";

            // Display the slot only if the quantity is greater than 0
            itemSlot.style.display = itemInstance.quantity > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    // Toggle the visibility of the inventory UI.
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        root.style.display = isInventoryOpen ? DisplayStyle.Flex : DisplayStyle.None;
        Debug.Log("Inventory toggled: " + (isInventoryOpen ? "Open" : "Closed"));

        if (isInventoryOpen)
        {
            PopulateInventory();
        }
    }

    // Use an item (triggered by double-clicking its slot)
    void UseItem(InventoryItemInstance itemInstance)
    {
        Debug.Log($"Used {itemInstance.itemData.itemName}");

        if (itemInstance.quantity > 0)
        {
            // Decrease the quantity (simulate consumption)
            itemInstance.quantity--;

            // If the item has a healing value, apply it (insert your player-healing logic here)
            if (itemInstance.itemData.healing > 0)
            {
                Debug.Log($"Healing for {itemInstance.itemData.healing} points!");
            }

            UpdateInventoryUI();
        }
    }

    // Update the inventory UI (for example, after using or adding an item)
    void UpdateInventoryUI()
    {
        for (int i = 0; i < items.Length && i < itemSlots.Length; i++)
        {
            var itemInstance = items[i];
            var itemSlot = itemSlots[i];

            if (itemInstance == null || itemSlot == null)
                continue;

            itemSlot.text = $"{itemInstance.itemData.itemName} x{itemInstance.quantity}";
            itemSlot.style.display = itemInstance.quantity > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    // Add an item to the inventory (used by pickups)
    public void AddItem(InventoryItemData newItemData, int amount = 1)
    {
        Debug.Log($"Trying to add {amount} {newItemData.itemName}(s) to the inventory.");

        // 1. Check if the item already exists in the inventory.
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemData == newItemData)
            {
                items[i].quantity += amount;
                Debug.Log($"Increased quantity of {newItemData.itemName} to {items[i].quantity}.");
                UpdateInventoryUI();
                return;
            }
        }

        // 2. Find an empty slot or a slot with quantity <= 0.
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null || items[i].quantity <= 0)
            {
                items[i] = new InventoryItemInstance(newItemData, amount);
                Debug.Log($"Added {newItemData.itemName} to slot {i} with quantity {amount}.");
                UpdateInventoryUI();  // Ensure the UI updates when the item is added
                return;
            }
        }

        // 3. No empty slot found.
        Debug.LogWarning("No empty slot available to add the item!");
    }

    // Load a Sprite from the Resources folder using the icon name
    Sprite LoadIcon(string iconName)
    {
        return Resources.Load<Sprite>($"Icons/{iconName}");
    }
}
