# IDL4 to C# Language Mapping

This document describes the OpenDDSharp current implementation of the [IDL4 to C# Language Mapping Specification](https://www.omg.org/spec/IDL4-CSHARP/).

Some sections of the specification are not included in this documentation because are not directly related with DDS, therefore won't be implemented.

The following legend is used to define the current version status:

:white_check_mark: Fully implemented

:ballot_box_with_check: Partially implemented

:x: Not implemented

## Core Data Types

### Modules

:white_check_mark: IDL modules are mapped to C# namespaces of the same name.

:white_check_mark: All IDL type declarations within the IDL module shall be mapped to corresponding C# declarations within the generated namespace.

:white_check_mark: IDL declarations not enclosed in any module shall be mapped into the global scope.

### Constants
:ballot_box_with_check: IDL constants shall be mapped to `public sealed class`es of the same name within the equivalent scope and namespace where they are defined.

> [!NOTE]
> OpenDDSharp makes the class `static` instead of `sealed`. It makes no sense to create instances of the class.
> With the current proposal, the following code snippet is possible:
> ```csharp
> var myUglyCode = new PI();
> double myPi = myUglyCode.Value;
> ```

:ballot_box_with_check: The mapped `class` shall contain a `public const` called `Value` assigned to the value of the IDL constant.

> [!NOTE]
> The use of `const` could be problematic because when using `const` it does not access to the field but
> the value is copied on build-time with the same constant value, therefore you need to recompile all your software
> and dependencies to ensure you are using the same value of the constant everywhere.
> OpenDDSharp uses a `public static readonly` field instead, in order to only require compilation of your IDL library and
> the value will be changed to all software that reference that library without need to recompile it again.

## Data Types

### Basic Types

#### Integer Types

:ballot_box_with_check: IDL integer types shall be mapped as shown in the following table:

| IDL Type           | C# Type | Status             |
| ------------------ | ------- | ------------------ |
| int8               | sbyte   | :x:                |
| uint8              | byte    | :x:                |
| short              | short   | :heavy_check_mark: |
| int16              | short   | :heavy_check_mark: |
| unsigned short     | ushort  | :heavy_check_mark: |
| uint16             | ushort  | :heavy_check_mark: |
| long               | int     | :heavy_check_mark: |
| int32              | int     | :heavy_check_mark: |
| unsigned long      | uint    | :heavy_check_mark: |
| uint32             | uint    | :heavy_check_mark: |
| long long          | long    | :heavy_check_mark: |
| int64              | long    | :heavy_check_mark: |
| unsigned long long | ulong   | :heavy_check_mark: |
| uint64             | ulong   | :heavy_check_mark: |

#### Floating-Point Types

:ballot_box_with_check: IDL floating-point types shall be mapped as shown in the following table:

| IDL Type    | C# Type | Status             |
| ----------- | ------- | ------------------ |
| float       | float   | :heavy_check_mark: |
| double      | double  | :heavy_check_mark: |
| long double | decimal | :x:                |

#### Char Types

:white_check_mark: The IDL `char` type shall be mapped to the C# type `char`.

> [!NOTE]
> IDL characters are 8-bit quantities representing elements of a character set, while C# characters are
> 16-bit unsigned quantities representing Unicode characters in UTF-16 encoding.

#### Wide Char Types

:white_check_mark: The IDL `wchar` type shall be mapped to the C# type `char`.

#### Boolean

:white_check_mark: The IDL boolean type shall be mapped to the C# `bool`, and the IDL constants `TRUE` and `FALSE` shall be mapped to
the corresponding C# boolean literals `true` and `false`.

### Template Types

#### Sequences

:white_check_mark: IDL sequences shall be mapped to the C# System.Collections.Generic.IList<T> interface, instantiated with the mapped type T of the sequence elements.

:white_check_mark: In the mapping, everywhere the sequence type is needed, a System.Collections.Generic.IList<T> shall be used.

:white_check_mark: Implementations of System.Collections.Generic.IList<T> shall be writable.

:x: Bounds checking on bounded sequences may raise an exception if necessary.

:ballot_box_with_check: The following table shows the mapping for sequences of basic types:

| IDL Type                       | C# Type                                     | Status             |
| ------------------------------ | ------------------------------------------- | ------------------ |
| sequence\<boolean\>            | System.Collections.Generic.IList\<bool\>    | :heavy_check_mark: |
| sequence\<char\>               | System.Collections.Generic.IList\<char\>    | :heavy_check_mark: |
| sequence\<char\>               | System.Collections.Generic.IList\<char\>    | :heavy_check_mark: |
| sequence\<int8\>               | System.Collections.Generic.IList\<sbyte\>   | :x:                |
| sequence\<uint8\>              | System.Collections.Generic.IList\<byte\>    | :x:                |
| sequence\<octet\>              | System.Collections.Generic.IList\<byte\>    | :heavy_check_mark: |
| sequence\<short\>              | System.Collections.Generic.IList\<short\>   | :heavy_check_mark: |
| sequence\<int16\>              | System.Collections.Generic.IList\<short\>   | :heavy_check_mark: |
| sequence\<unsigned short\>     | System.Collections.Generic.IList\<ushort\>  | :heavy_check_mark: |
| sequence\<uint16\>             | System.Collections.Generic.IList\<ushort\>  | :heavy_check_mark: |
| sequence\<long\>               | System.Collections.Generic.IList\<int\>     | :heavy_check_mark: |
| sequence\<int32\>              | System.Collections.Generic.IList\<int\>     | :heavy_check_mark: |
| sequence\<unsigned long\>      | System.Collections.Generic.IList\<uint\>    | :heavy_check_mark: |
| sequence\<uint32\>             | System.Collections.Generic.IList\<uint\>    | :heavy_check_mark: |
| sequence\<long long\>          | System.Collections.Generic.IList\<long\>    | :heavy_check_mark: |
| sequence\<int64\>              | System.Collections.Generic.IList\<long\>    | :heavy_check_mark: |
| sequence\<unsigned long long\> | System.Collections.Generic.IList\<ulong\>   | :heavy_check_mark: |
| sequence\<uint64\>             | System.Collections.Generic.IList\<ulong\>   | :heavy_check_mark: |
| sequence\<float\>              | System.Collections.Generic.IList\<float\>   | :heavy_check_mark: |
| sequence\<double\>             | System.Collections.Generic.IList\<double\>  | :heavy_check_mark: |
| sequence\<long double\>        | System.Collections.Generic.IList\<decimal\> | :heavy_check_mark: |

#### Strings

:white_check_mark: IDL `string`s, both bounded and unbounded variants, shall be mapped to C# `string`s.

:white_check_mark: The resulting strings shall be encoded in UTF-16 format.

#### Wstrings

:white_check_mark: IDL `wstring`s, both bounded and unbounded variants, shall be mapped to C# `string`s.

:white_check_mark: The resulting strings shall be encoded in UTF-16 format.

#### Fixed Type

:x: The IDL fixed type shall be mapped to the C# decimal type.

:x: Range checking may raise a System.ArithmeticException exception if necessary.

### Constructed Types

#### Structures

:white_check_mark: An IDL struct shall be mapped to a C# `public class` with the same name.

:ballot_box_with_check: The `class` shall provide the following:

- :white_check_mark: A public property of the equivalent type for each member of the structure, including both a getter and a setter.
  - :x: In general, properties representing IDL sequences and maps shall include only a getter.
  - :x: As an exception, properties representing sequences and maps that are marked with the `@external`
        annotation (see the Standardized Annotations building block) shall include both a getter and a setter.
- :ballot_box_with_check: Property setters shall perform shallow assignments of reference types and deep copies of value types.
- :white_check_mark: A public default constructor that takes no parameters
- :x: A public copy constructor that takes as a parameter an object of the mapped class.
  - :x: The copy constructor shall perform a deep copy of every member of the structure.
  - :x: Implementations supporting the Standardized Annotations building block shall perform a shallow assignment for members annotated with `@external`
- :x: A public constructor that accepts parameters for each member (i.e., the all values constructor). The
constructor shall perform shallow assignments of reference types and deep copies of value types.

:white_check_mark: The default constructor shall initialize member fields as follows:
- :white_check_mark: All primitive members shall be left as initialized by the C# default initialization.
- :white_check_mark: All string members in the struct shall be initialized to string.Empty.
- :white_check_mark: All array members shall be initialized to an array of the declared size whose elements are initialized with their default constructor.
- :white_check_mark: All sequence members shall be initialized to zero-length sequences of the corresponding type.
- :white_check_mark: All other members shall be initialized to an object created with their respective default constructor.

:x: The class shall implement the `IEquatable<T>` interface, where `T` is the corresponding class name.

#### Unions

:x: An IDL union shall be mapped to a C# `public class` with the same name.

:x: The class shall provide the following:
- :x: A public default constructor.
- :x: A public copy constructor that takes as a parameter an object of the mapped class. The copy constructor shall perform a deep copy of the member selected by the discriminator, if any.
  - :x: Implementations supporting the Standardized Annotations shall perform a shallow assignment if the selected member is annotated with `@external`
- :x: A public read-only property named `Discriminator`.
- :x: A public property with getters and setters for each member. Property setters shall perform shallow assignments of reference types and deep copies of value types.
  - :x: In general, properties representing IDL sequences and maps, shall include only a getter. In such cases:
    - :x: For every sequence member, the union shall define two modifier methods. The first modifier
      method shall have the following prototype: void Set<SequenceMemberName>(). The method
      shall clear the sequence and update the discriminator value. The second modifier method shall have
      the following prototype: `void Set<SequenceMemberName>(System.Collections.IEnumerable<T> elements)`, where `T`
      is the equivalent type of the IDL sequence elements. The method shall clear the sequence, populate
      it with the elements received as an input, and update the discriminator value.
    - :x: For every map member, the union shall define two modifier methods. The first modifier method
      shall have the following prototype: `void Set<MapMemberName>()`. The method shall remove all
      the elements in the property representing the map and update the discriminator value. The second
      modifier method shall have the following prototype: 
      `void Set<MapMemberName>(System.Collections.IEnumerable<Generic.KeyValuePair<TKey,Tvalue>> elements)`, 
      where `TKey` is the equivalent key type, and `TValue` is the equivalent value type.
      The method shall remove all elements in the equivalent map, populate it with the
      elements received as an input, and update the discriminator value.
  - :x: As an exception, properties representing sequences and maps that are marked with the `@external`
    annotation (see the Standardized Annotations building block) shall include both a getter and a setter.
- :x: A public property with getters and setters for the member corresponding to the default label, if present.

:x: The normal name conflict resolution rule shall apply (i.e., prepend an "_") to the discriminator property name if
there is a name clash with the mapped union type name or any of the field names.

:x: Property getters shall raise a System.InvalidOperationException if the expected member has not been set.

:x: If there is more than one case label corresponding to a member, the setter of the property representing such member
shall set Discriminator to the first possible case label. If the member corresponds to the default case label, then
`Discriminator` shall be set to the first available default value starting from the zero-index of the discriminant type.
For all such members, the union shall provide a modifier method void `Set<MemberName>(<MemberType> value, <DiscriminatorType> discriminator)` to set 
the corresponding property value and the discriminator value of choice.

:x: The modifier method shall throw a System.ArgumentException exception when a value is passed for the discriminator that is not among the case labels for the member.4

:x: The class representing the IDL union shall implement the `IEquatable<T>` interface, where `T` is the corresponding class name.

#### Enumerations

:white_check_mark: An IDL `enum` shall be mapped to a C# `public enum` with the same name as the IDL enum type.

:x: If the IDL enumeration declaration is preceded by a `@bit_bound` annotation; the corresponding C# enum type shall
be `sbyte` for bit bound values between 1 and 8; `short`, for bit bound values between 9 and 16; `int`, for bit bound
values between 17 and 32; and `long`, for bit bound values between 33 and 64.

### Arrays

:white_check_mark: An IDL array shall be mapped to a C# array of the mapped element type or to a C# `class` offering an interface
compatible with that of a C# native array of the mapped element type. 

:white_check_mark: In the mapping, everywhere the array type is needed, an array or an equivalent class of the mapped element type shall be used.

:x: The bounds for the array shall be checked by the setter of the corresponding property and a `System.ArgumentOutOfRangeException` shall be
raised if a bounds violation occurs.

### Naming Data Types

:white_check_mark: C# does not have a `typedef` construct; therefore, the declaration of types using `typedef` in IDL shall not result in
the creation of any C# type. Instead, the use of an IDL `typedef` type shall be replaced with the type referenced by
the `typedef` statement. For nested `typedef`s, the `typedef`ed type shall be replaced with the original type in the
sequence of `typedef` statements.

## Any

:x: The IDL any type shall be mapped to `Omg.Types.Any` type. The implementation of the `Omg.Types.Any` is
platform-specific, and should include operations that allow programmers to insert and access the value contained in
an any instance as well as the actual type of that value.

## Extended Data Types

### Structures with Single Inheritance

:white_check_mark: An IDL `struct` that inherits from a base IDL `struct`, shall be declared as a
C# `public class` that extends the `class` resulting from mapping the base IDL `struct`.

:ballot_box_with_check: The resulting C# `public class` shall be mapped according to the general mapping rules for IDL `struct`s with the following additions:
- :x: The public copy constructor shall call the "all values constructor" of the base class with the value of the
members in the new instance that are derived from the base IDL `struct`.
- :x: The public "all values constructor" shall take as parameters an object of the base class, followed parameters
for each member of the IDL struct. The "all values constructor" shall call the copy constructor of the base
class using the object of the base class provided as a parameter.

### Union Discriminators

:x: This building block adds the `int8`, `uint8`, `wchar`, and `octet` IDL types to the set of valid types for a discriminator.

### Additional Template Types

#### Maps

:x: An IDL `map` shall be mapped to a C# generic `System.Collections.Generic.IDictionary<TKey,TValue>`
instantiated with the equivalent C# key type and value type. In the mapping, everywhere the `map` type is needed, a
property of type `IDictionary` with the equivalent C# key type and value type shall be used.

:x: Bounds checking shall raise an exception if necessary.

#### Bitsets

:x: An IDL `bitset` shall map to a C# `struct` with public properties for each named `bitfield` in the set. The IDL type
of each `bitfield` member, if not specified in the IDL, shall take the smallest unsigned integer type able to store the
bit field with no loss; that is, `byte` if it is between 1 and 8, `ushort` if it is between 9 and 16, `uint` if it is between 17
and 32 and `ulong` if it is between 33 and 64.

:x: The mapped C# struct shall implement the `IEquatable<T>` interface, where `T` is the corresponding `bitset` name.

#### Bitmask Type

:x: The IDL `bitmask` type shall map to a C# `public enum` with the same name, followed by the `Flags` suffix.

:x: In the mapping, everywhere the `bitmask` type is needed, a `System.Collections.BitArray` shall be used.

:x: The C# `enum` shall have the `System.FlagsAttribute`, and shall contain a literal for each named member of the IDL `bitmask`.

:x: The value of each C# `enum` literal is dictated by the `@position` annotation of the corresponding IDL `bitmask` member.

:x: If no position is specified, the C# `enum` literals shall be set to the value of the next power of two.

:x: The corresponding `enum` literals can be used to set, clear, and test individual bits in the corresponding `System.Collections.BitArray` instance.

:x: The size (number of bits) held in the `bitmask` determines the corresponding C# `enum` type. In particular, the `enum`
type shall be `byte`, for bit bound values between 1 and 8; `ushort`, for bit bound values between 9 and 16; `uint`, for
values between 17 and 32; and `ulong` for bit bound values between 33 and 64.

## Annotations

### Defining Annotations

:x: User-defined annotations are propagated to the generated code as C# attributes inheriting from the
`System.Attribute` class. The name of the corresponding attributes shall be that of the original IDL annotation, 
appending the `Attribute` suffix when applying the .NET Framework Design Guidelines Naming Scheme.

:x: Each annotation member shall be mapped to a property with public getters and setters. Moreover, the mapped
attribute shall have a public constructor with default values (default constructor) and shall be annotated with the
following attribute: `[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]`.

:x: If the IDL annotation definition provides a default value for a given member, it shall be reflected in the C# definition
accordingly; otherwise, the equivalent C# definition shall have no default value.

### Applying Annotations

:x: IDL elements annotated with user-defined annotations shall map to equivalent C# elements annotated with the
corresponding attribute following the mappings defined in this specification.

#### Applying Annotations in Naming Data Types

:x: Annotations on an IDL `typedef` shall be applied to uses of the `typedef` in other type declarations.

## Standardized Annotations

OMG-IDL4 defines some annotations and assigns them to logical groups. These annotations may be applied to
various constructs throughout an IDL document, and their impact on the language mapping is dependent on the
context in which they are applied. The following table summarize the annotations that have an impact in the 
C# language mapping and the current OpenDDSharp implementation status.

| General Purpose Annotation | Status |
| -------------------------- | ------ |
| `@optional`                | :x:    |
| `@position`                | :x:    |
| `@value`                   | :x:    |
| `@key`                     | :x:    |
| `@default_literal`         | :x:    |
| `@default`                 | :x:    |
| `@range`                   | :x:    |
| `@min`                     | :x:    |
| `@max`                     | :x:    |
| `@unit`                    | :x:    |
| `@bit_bound`               | :x:    |
| `@external`                | :x:    |
| `@verbatim`                | :x:    |

# IDL to C# Language Mapping Annotations

This chapter defines specialized annotations that extend the standard set defined in OMG-IDL4 to control the C# code generation.

## Annotation `@csharp_mapping`

:x: This annotation provides the means to customize the way a number of IDL constructs are mapped to the C# programming language.

The `@csharp_mapping` annotation provides three parameters described in the following sections.

### Parameter `apply_naming_convention`

:x: `apply_naming_convention` specifies whether the IDL to C# language mapping shall apply the IDL Naming
Scheme or the .NET Framework Design Guidelines Naming Scheme when mapping IDL names to C#. In particular:
- :x: If `apply_naming_convention` is `IDL_NAMING_CONVENTION`, the code generator shall generate type
identifiers and names according to the IDL Naming Scheme, leaving the name of the corresponding IDL
construct unchanged.
- :x: If `apply_naming_convention` is `DOTNET_NAMING_CONVENTION`, the code generator shall generate type
identifiers and names according to the .NET Framework Design Guidelines Naming Scheme.

### Parameter `constants_container`

:x: `constants_container` activates the different options for mapping constants. To enable the
Standalone Constants Mapping, `constants_container` shall be set to an empty string.
To enable the Constants Container Mapping , `constants_container` shall bet set to a
valid string. The default name for the containing class is `Constants`.

### Parameter `struct_type`

:x: `struct_type` defines the C# type the IDL `struct` type map to. By default, IDL
`struct`s are mapped to a C# `class`. This parameter allows changing the default behavior to map an IDL `struct`
to a C# `struct`. When mapping an IDL `struct` to C# `struct` as a result of this annotation, every setter and constructors shall
perform a deep copy, regardless of annotations modifying the copy behavior, such as `@external`.