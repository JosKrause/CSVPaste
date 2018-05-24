﻿using System;

namespace CSVPaste
{
    /// <summary>
    /// Helper class for working with the value types.
    /// </summary>
    internal class ValueTypeHelper
    {
        /// <summary>
        /// Determines the <see cref="ValueType"/> of the values and the existence of a header.
        /// </summary>
        /// <param name="values">The raw values.</param>
        /// <param name="header">Flag indicating whether the column header was present in the values or not.</param>
        public static ValueType DetermineValuesType(string[] values, out bool header)
        {
            header = false;

            // Determine the type of the last item (the first item from the back).
            // If the type is text, treat all the values as text.
            var initialType = GetValueType(values[values.Length - 1]);
            if (initialType == ValueType.Text)
                return ValueType.Text;

            // If the array contains only one element, return its type.
            if (values.Length == 1)
                return initialType;

            // For every item except the first and the last, check if their type matches the initially determined one.
            // If is doesn’t, treat all the values as text.
            for (var i = values.Length - 2; i >= 1; i--)
            {
                if (!EqualsValueType(values[i], initialType))
                {
                    return ValueType.Text;
                }
            }

            // Determine the type of the first item.
            // If the first item is text but none of the previous items is, then the selection contains a header.
            var firstItemType = GetValueType(values[0]);
            if (firstItemType == ValueType.Text)
            {
                header = true;
                return initialType;
            }

            // If the first item is not text but it still doesn’t match the type of the previous items,
            // then there are different types of items in the selection.
            // Treat all of them as text.
            if (!EqualsValueType(values[0], initialType))
            {
                return ValueType.Text;
            }

            return initialType;
        }

        /// <summary>
        /// Gets the <see cref="ValueType"/> of the value.
        /// </summary>
        /// <param name="value">The raw value.</param>
        private static ValueType GetValueType(string value)
        {
            long longValue;
            if (long.TryParse(value, out longValue))
                return ValueType.Numeric;

            Guid guidValue;
            if (Guid.TryParse(value, out guidValue))
                return ValueType.Uniqueidentifier;

            return ValueType.Text;
        }

        /// <summary>
        /// Determines whether the value is of particular <see cref="ValueType"/>.
        /// </summary>
        /// <param name="value">The raw value.</param>
        /// <param name="valueType">The <see cref="ValueType"/>.</param>
        private static bool EqualsValueType(string value, ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.Numeric:
                    long longValue;
                    return long.TryParse(value, out longValue);
                case ValueType.Uniqueidentifier:
                    Guid guidValue;
                    return Guid.TryParse(value, out guidValue);
            }

            return true;
        }
    }
}